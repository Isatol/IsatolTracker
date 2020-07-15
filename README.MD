## Package Tracker

Track packages from UPS, Fedex and Estafeta MX.

### Run the project

Open the **Tracker.sln** solution and wait for intall all dependencies.

Tracker project is a class library, so you need to include that library in your own project.

Run the TestTracker project to test if everything is ok.

### Using the library

```cs
TrackingModel trackingFedex = IsatolTracker.Track.Fedex("123456789");
TrackingModel trackingEstafeta = IsatolTracker.Track.Estafeta("123456789")
```
