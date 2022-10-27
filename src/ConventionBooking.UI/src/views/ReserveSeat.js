import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom"
import { Alert } from "reactstrap";
import history from "../utils/history";
import Highlight from "../components/Highlight";
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import { getConfig } from "../config";
import Loading from "../components/Loading";
import ReserveSeat from "../components/ReserveSeat";

export const ReserveSeatComponent = () => {
  const { audience, apiConvention = "http://localhost:3001" } = getConfig();

  const search = useLocation().search;
  const id = new URLSearchParams(search).get('id');

  const [state, setState] = useState({
    showResult: false,
    apiMessage: "",
    error: null,
  });

  const [title, setTitle] = useState("");
  const [description, setDescription]= useState("");
  const [numberOfSeats, setNumberOfSeats]= useState("");

  // Simplistic load atleast once. also has the side effect or reloading the list when moving from page to page
  useEffect(() => listTalks(id), []); // eslint-disable-line react-hooks/exhaustive-deps

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

    await listTalks(id);
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

    await listTalks(id);
  };

  const listTalks = async (id) => {
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
            if (ev.talk.id === id) {
                _event = ev;
            }
        });

        setTitle(_event.talk.title);
        setDescription(_event.talk.description);
        setNumberOfSeats(_event.numberOfSeats);

    } catch (error) {
        console.error("listTalks", error);
        setTitle("");
        setDescription("");
        setNumberOfSeats("");
    }
  };

  const signupForTalk = async (talkId) => {
    try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${apiConvention}/registrations/seats`, {
          method: 'POST',
          headers: {
            Authorization: `Bearer ${token}`,
            'Content-Type': "application/json"
          },
          body: JSON.stringify({"id": talkId})
        });

        const responseData = await response.json();

        setState({
          ...state,
          showResult: false,
          apiMessage: responseData,
        });


        // look for ok response and redirect back to talks.
        history.push('/talks');
    } catch (error) {
        setState({
          ...state,
          error: error.error,
        });

        console.error("signupForTalk", error);
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

        <h1>Reserve a Seat</h1>
        <p className="lead">
          You still have the oppportunity to reserve a seat for this talk.
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

        <ReserveSeat id={id} title={title} description={description} remaningSeats={numberOfSeats} signupHandler={() => signupForTalk(id)}/>
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

export default withAuthenticationRequired(ReserveSeatComponent, {
  onRedirecting: () => <Loading />,
});
