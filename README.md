# nw-func

This function app is triggered by HTTP (from Postman) and send a new message (with session ID) to service bus. The goal is to test the latency between Function App and Service Bus.
What we have found out is that if we use function app's output binding, the latency is constant around 20ms, 
but if we use the SDK, the latency is much higher, around 150ms and could burst over 200ms.
