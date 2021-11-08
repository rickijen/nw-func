# nw-func

This function app is triggered by HTTP (from running Postman collection) and send a new message (with session ID) to service bus. The goal is to test the latency between Function App and Service Bus.
What we have found out is that if we use function app's output binding, the latency is constant around 20ms, but if we use the SDK, the latency is much higher, around 150ms and could burst over 200ms.

![pic1](https://user-images.githubusercontent.com/15071173/140669043-dd330ee9-25ba-47d2-81bc-c85de2cae77a.png)

![pic2](https://user-images.githubusercontent.com/15071173/140669050-9a508ace-0872-4a48-887e-7ceffc9deb8e.png)
