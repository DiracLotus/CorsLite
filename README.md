# CorsLite

## Key Features:
- MVC .NET 5.0 for the not-so beautiful front end
- SignalR to push messages to the client, to keep them updated on the status of their chosen beverage
- Kept business seperate to the web end (there's still a bit too much coupling for my liking)
- Command abstractions to allow for a) talking to external components (to actually make the coffee) and b) allow easy swapping in and out if recipes change
