# Answers To Posed Questions
- [x] Explanation of the certified library you know the best and you would recommend
  * Server libraries: (C#, Duende Identity Server), (Golang, Ory Hydra), (Java, Keycloak)
  * Client libraries: (C#, IdentityModel), (React SPA, auth0-react), (Golang, golang/oauth2)
- [x] Create Context and Container level diagrams of the solution in C4 format (https://c4model.com)
- [] Explanation of grant types (flows). Which should be used when and when not?
  * Authorization Code Grant flow


  * Authorization Code Grant flow With PKCE
    The added PCKE to the flow prevents authorization code injection by using Proof-Key for Code Exchange,
    meaning the Initiating Party generates a secret key on the public client and calculates a cryptographic hash as of the secret, and puts this hash in the authorization request. Upon exchanging the authorization code for a token using an https request, the secret is added, so the token server can verify that it was indeed this client session that performed the flow by redoing the calculation and matching it to the hash it got sent.

  * Hybrid flow
    Mandates a nonce.

  * Implicit flow
    Mandates a nonce. Deprecated should not be used, use Authorization Code flow with PKCE instead.

  * Device Code Grant flow
    Used for devices with limited input device options. The client starts a flow and receives a code for dispaying to the user. Client will poll on the flow to check if user authenticated and authorized.

  * Refresh Token Grant flow
    Using https a refresh token is exchanged for a new access token.

  * Client Credentials Grant flow
    The client is considered confidential and able to protect a secret. The flows issues access tokens in exchange for value client_id and client_secret values. This flow requires a secure means of transportation often https for web requests.

  * Resource Owner Password Grant flow.
    Deprecated should not be used, use Client Credentials flow instead.

- [x] Explain the choice of user flow for web
  * Use Authorization Code flow with a Confidential client
  * Unless its web that serve SPA then same flow with public client and PKCE added.

- [x] Explain the choice of user flow for mobile
  * Use Authorization Code flow with PKCE and public client

- [] Explain how a user flow works

- [x] Explain what the concepts do and who owns then
  * PKCE, Proof-Key for Code Exchange ensures client starting the flow is also the one exchanging for the access token. Prevents CSRF and authorization code injection attacks
  * Nonce, a case sensitive string owned by the client and must be verified by the client by matching to the nonce parameter sent in the authentication request. Protects against CSRF and mitigates replay attacks.
  * State, is a random string sent during the initial authorization request owned by the client. It protects against CSRF attacks.
  * Redirect URI is registered to the client often at time of creation, owned by the client, but enforced by the IDP to prevent redirect misdirection attacks.

- [x] Explain the duties and roles of clients, resources, the authority and the browser
  * clients represent an application or multiple applications
  * resources are the thing we are trying to protect
  * the authority
  * the browser

- [x] Explain how trust is establised between a web server and the IDP
  * End to end encryption
  * Certificate validation
  * Data signing

- [] Explain how custom scopes could be used in this app
   * What benefits do they provide
   * What disadvantages
   * What do they protect against
   * How are scopes connected to JWT

- [x] A developer is implementing a solution and wants to know wether a user is logged in, how can that be achived?
  * Use prompt=none in an authentication request, this will act exactly as a normal login without user interaction if successful or give `login_required`, `consent_required` or `interaction_required` response
- [x] If done in a browser client-side, how is the communication protected?
  * Using https, HttpOnly secure cookies, CORS and cache control set to no-cache.
