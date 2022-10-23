# Problem Statement

Develop and present a solution that facilitates conventions with a series of talks.

## Focus on the following aspects when creating the solution

* Authentication and Authorization using OpenID Connect
* Security Implications
* Privacy and Legal constraints
* High performance and availability to handle high concurrent users in short time

## Scope Reduction
* A complete solution is not expected, stub the parts that are of little importance of the focus area
* Choose to focus on the parts that reflect your skillset and area of interest
* Frontend is not the focus area

## Constraints

[] Solution must be written in C#
[] Sulution must use OpenID Connect
[] Use React or similar for Frontend
[] Explanation of the certified library chosen
[] Create Context and Container level diagrams of the solution in C4 format (https://c4model.com)

[] Explain Grant Type flows, which should be used and when not to use it
  [] Client Credentials Grant
  [] Authorization Code Grant
     [] With PKCE
  [] Device Code Grant
  [] Refresh Token Grant
  [] Resource Owner Password Grant
  [] Implicit Grant

[] Explain the choice of user flow for web
[] Explain the choice of user flow for mobile
[] Explain how a user flow works
[] Explain what the concepts do and who owns then
  [] PKCE
  [] A Nonce
  [] State
  [] Redirect URI
[] Explain the duties and roles of clients, resources, the authority and the browser
[] Explain how trust is establised between a web server and the IDP
[] Explain how custom scopes could be used in this app
   [] What benefits do they provide
   [] What disadvantages
   [] What do they protect against
   [] How are scopes connected to JWT
[] A developer is implementing a solution and wants to know wether a user is logged in, how can that be achived?
  [] If done in a browser client-side, how is the communication protected?



## Discussion of Security Implications

https://openid.net/specs/openid-connect-core-1_0.html#Security
