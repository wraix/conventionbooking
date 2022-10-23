# Reasoning for decisions

This documents decisions made along the way when designing and building the Case Study for Convention Booking System.

## Considerations for choosing an Identity Provider

[x] OIDC certified
[x] Free personal plan for development
[x] Market leading
[x] Able to scale when application requirements change
[x] Certified client libraries provided for React

I have choosen to use Auth0 as per suggestion, to not have to build an entire Identity Provider and it is free of charge and requires no creditcard information to get started.
The focus of the assignment is directed at the securing of an application using an Identity Provider, and to ensure the application scales to many concurrent users. In this case it will be capped at 7000 concurrent users by the IDP Free for Developers plan.

Auth0 should be a good choice when wanting to scale later as they are market leading and part of Okta, that is actually a key player in defining all the OAuth and OIDC specifications. It will how ever require an Enterprise account and probably some infrastructure configuration
to truly allow for scalability of system to millions of concurrent users or it might require to upgrade to its big sister Okta.

Auth0 also provides certified client libraries to build applications for most languages and in this particular case a library for React. Do not build your own client library as the oauth protocol is quite hard to get right and can cause severe security incidents if implemented wrong.
Alternatively one can use a client library on the list here: https://openid.net/developers/certified/

## ?
