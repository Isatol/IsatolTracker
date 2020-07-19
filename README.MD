## Package Tracker

Track packages from UPS, Fedex and Estafeta MX.

### Run the project

Open the **Tracker.sln** solution and wait for intall all dependencies.

Tracker project is a class library, so you need to include that library in your own project.

Run the TestTracker project to test if everything is ok.

### Using the library

```cs
HttpClient httpClient = new HttpClient();
Isatol.Tracker.Track track = new Isatol.Tracker.Track(httpClient);
TrackingModel estafeta = await track.Estafeta("123456");
TrackingModel ups = await track.UPS("123456", Isatol.Tracker.Track.Locale.es_MX);
TrackingModel fedex = await track.Fedex("123456");
```
