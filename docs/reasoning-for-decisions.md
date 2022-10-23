# Reasoning for decisions

[x] Choice of Identity Provider

## Choice of Identity Provider

I have choosen to use Auth0 as per suggestion, to not have to build an entire Identity Provider and it is free of charge and requires not creditcard.
The focus of the assignment is directed at the securing of an application using an Identity Provider, and to ensure the application scales to many concurrent users. In this case it will be capped at 7000 concurrent users by the IDP.

Auth0 should be a good choice when wanting to scale later as they are market leading. It will how ever require an Enterprise account and probably some infrastructure configuration
to truly allow for scalability of system to millions of concurrent users.

Restrictions:
 * Free personal plan only allows for 7000 users, but unlimited logins



