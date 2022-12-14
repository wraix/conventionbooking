import React, { useState } from "react";
import { Button, Alert } from "reactstrap";
import Highlight from "../components/Highlight";
import RegistrationList from "../components/RegistrationList";
import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import { getConfig } from "../config";
import Loading from "../components/Loading";

export const WhatYouWantComponent = () => {
  const { audience, apiConvention = "http://localhost:3001" } = getConfig();

  const [state, setState] = useState({
    showResult: false,
    apiMessage: "",
    error: null,
  });

  const [registrations, setRegistrations] = useState([]);

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

    await listRegistrations();
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

    await listRegistrations();
  };

  const listRegistrations = async () => {
    try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${apiConvention}/talks`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const responseData = await response.json();
        setRegistrations(responseData); // replace the state with new state from api

    } catch (error) {
        console.error("listTalks", error);
        setRegistrations([]);
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

        <h1>What you want</h1>
        <p className="lead">
          This is what you want to attend and have signed up for or reserved seats at.
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
          onClick={listRegistrations}
          disabled={!audience}
        >
          Refresh
        </Button>
        <RegistrationList data={registrations} />
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

export default withAuthenticationRequired(WhatYouWantComponent, {
  onRedirecting: () => <Loading />,
});
