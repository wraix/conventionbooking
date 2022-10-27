import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom"
import { Alert } from "reactstrap";
import history from "../utils/history";
import Highlight from "../components/Highlight";
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import { getConfig } from "../config";
import Loading from "../components/Loading";
import Signup from "../components/Signup";

export const SignupComponent = () => {
  const { audience, apiConvention = "http://localhost:3001" } = getConfig();

  const search = useLocation().search;
  const id = new URLSearchParams(search).get('id');

  const [state, setState] = useState({
    showResult: false,
    apiMessage: "",
    error: null,
  });

  const [name, setName] = useState("");
  const [venue, setVenue]= useState("");

  // Simplistic load atleast once. also has the side effect or reloading the list when moving from page to page
  useEffect(() => listConventions(id), []); // eslint-disable-line react-hooks/exhaustive-deps

  const {
    getAccessTokenSilently,
    loginWithPopup,
    getAccessTokenWithPopup,
  } = useAuth0();

  const handleConsent = async () => {
    try {
      await getAccessTokenWithPopup();
      setState({
        ...state,
        error: null,
      });
    } catch (error) {
      setState({
        ...state,
        error: error.error,
      });
    }

    await listConventions(id);
  };

  const handleLoginAgain = async () => {
    try {
      await loginWithPopup();
      setState({
        ...state,
        error: null,
      });
    } catch (error) {
      setState({
        ...state,
        error: error.error,
      });
    }

    await listConventions(id);
  };

  const listConventions = async (id) => {
    try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${apiConvention}/events`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const responseData = await response.json();

        // filter events by talk id
        let _event = {};
        responseData.forEach(ev => {
            if (ev.convention.id === id) {
                _event = ev;
            }
        });

        setName(_event.convention.name);
        setVenue(_event.venue.name);

    } catch (error) {
        console.error("listConventions", error);
        setName("");
        setVenue("");
    }
  };

  const signupForConvention = async (conventionId) => {
    try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${apiConvention}/registrations/conventions`, {
          method: 'POST',
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': "application/json"
          },
          body: JSON.stringify({"id": conventionId})
        });

        const responseData = await response.json();

        setState({
          ...state,
          showResult: false,
          apiMessage: responseData,
        });


        // look for ok response and redirect back to talks.
        history.push('/conventions');
    } catch (error) {
        setState({
          ...state,
          error: error.error,
        });

        console.error("signupForConvention", error);
    }
  }

  const handle = (e, fn) => {
    e.preventDefault();
    fn();
  };

  return (
    <>
      <div className="mb-5">
        {state.error === "consent_required" && (
          <Alert color="warning">
            You need to{" "}
            <a
              href="#/"
              class="alert-link"
              onClick={(e) => handle(e, handleConsent)}
            >
              consent to get access to users api
            </a>
          </Alert>
        )}

        {state.error === "login_required" && (
          <Alert color="warning">
            You need to{" "}
            <a
              href="#/"
              class="alert-link"
              onClick={(e) => handle(e, handleLoginAgain)}
            >
              log in again
            </a>
          </Alert>
        )}

        <h1>Signup for Convention</h1>
        <p className="lead">
          You still have the oppportunity to go to this Convention.
        </p>

        {!audience && (
          <Alert color="warning">
            <p>
              The application is missing the configuration for audience, or it is
              using the default value of <code>YOUR_API_IDENTIFIER</code>.
              Please update it to point to the Convention Booking API.
            </p>
          </Alert>
        )}

        <Signup id={id} name={name} venue={venue} signupHandler={() => signupForConvention(id)}/>
      </div>

      <div className="result-block-container">
        {state.showResult && (
          <div className="result-block" data-testid="api-result">
            <h6 className="muted">Result</h6>
            <Highlight>
              <span>{JSON.stringify(state.apiMessage, null, 2)}</span>
            </Highlight>
          </div>
        )}
      </div>
    </>
  );
};

export default withAuthenticationRequired(SignupComponent, {
  onRedirecting: () => <Loading />,
});
