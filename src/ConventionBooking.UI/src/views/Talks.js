import React, { useState, useEffect } from "react";
import { Button, Alert } from "reactstrap";
import Highlight from "../components/Highlight";
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import { getConfig } from "../config";
import Loading from "../components/Loading";
import TalksList from "../components/TalksList";

export const TalksComponent = () => {
  const { audience, apiConvention = "http://localhost:3001" } = getConfig();

  const [state, setState] = useState({
    showResult: false,
    apiMessage: "",
    error: null,
  });

  const [talks, setTalks] = useState([]);

  const {
    getAccessTokenSilently,
    loginWithPopup,
    getAccessTokenWithPopup,
  } = useAuth0();

  // Simplistic load atleast once. also has the side effect or reloading the list when moving from page to page
  useEffect(() => listTalks(), []); // eslint-disable-line react-hooks/exhaustive-deps

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

    await listTalks();
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

    await listTalks();
  };

  const listTalks = async () => {
    try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${apiConvention}/events`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const responseData = await response.json();

        // group events by talk
        let _talks = {};
        responseData.forEach(ev => {
          _talks[ev.talk.id] = ev.talk;
        });
        console.log(_talks)

        setTalks(Object.values(_talks)); // replace the state with new state from api

    } catch (error) {
        console.error("listTalks", error);
        setTalks([]);
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

        <h1>Talks</h1>
        <p className="lead">
          Look trough all the talks by pasionate brew master and find one to attend.
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
              onClick={listTalks}
              disabled={!audience}
        >
          Refresh
        </Button>
        <TalksList data={talks} />
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

export default withAuthenticationRequired(TalksComponent, {
  onRedirecting: () => <Loading />,
});
