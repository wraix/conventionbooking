import React, { useEffect, useState } from "react";
import { Alert, Button } from "reactstrap";
import Highlight from "../components/Highlight";
import ConventionList from "../components/ConventionList";
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import { getConfig } from "../config";
import Loading from "../components/Loading";

export const ConventionsComponent = () => {
  const { audience, apiConvention = "http://localhost:3001" } = getConfig();

  const [state, setState] = useState({
    showResult: false,
    apiMessage: "",
    error: null,
  });

  const [conventions, setConventions] = useState([]);

  const {
    getAccessTokenSilently,
    loginWithPopup,
    getAccessTokenWithPopup,
  } = useAuth0();

  // Simplistic load atleast once. also has the side effect or reloading the list when moving from page to page
  useEffect( () => listConventions(), []); // eslint-disable-line react-hooks/exhaustive-deps

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

    await listConventions();
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

    await listConventions();
  };

  const listConventions = async () => {
    try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${apiConvention}/conventions`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const responseData = await response.json();
        setConventions(responseData); // replace the state with the new state from api

    } catch (error) {
        console.error(error);
        setConventions([]);
    }
  };

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

        <h1>Conventions</h1>
        <p className="lead">
          Look trough all the conventions we have prepared for you and signup for the ones you find interesting.
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

        <Button
              color="primary"
              className="mt-5"
              onClick={listConventions}
              disabled={!audience}
        >
          Refresh
        </Button>
        <ConventionList data={conventions}/ >
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

export default withAuthenticationRequired(ConventionsComponent, {
  onRedirecting: () => <Loading />,
});
