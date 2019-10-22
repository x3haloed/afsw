# afsw
Absurdly Fast and Simple static Web framework

## Philosophy
AFSW believes that all resources should be rendered to static files as soon as possible. It accomplishes this with dynamic data by rendering and storing HTML shortly after content is submitted. This is achieved by roughly following the CQRS pattern. Resources are read separately from where they are written. Writes happen to a key value store and are then queued to be rendered to static files. All reads occur on static files.

I'm focusing on making AFSW work for a simple blogging use case where content can be created, updated, and commented on. After that's working well, I'll move onto more complicated use cases.

## Help!!
I need smart engineers to help me improve the system. Please suggest anything that might help improve it.
