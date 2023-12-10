#  Here be use cases (in our case command handlers) - incoming port interfaces
## The incoming port interface, strictly speaking, is what is made public by swagger - in  other words the OpenApi REST interface
- behind that OpenApi interface lie "thin controllers" that delegate the incoming calls to a mediator which then routes them to an appropriate command handler
- hence -> the command handlers are the application component that holds the logic relevant to a specific use-case