# KaptainsLog

 "Captain's log, Stardate 43539.1. We have moved into orbit around Bre'el IV. With the assistance of the planet's emergency control center, we are investigating a catastrophic threat to the population from a descending asteroidal moon.""

This mod REQUIRES and depends on the ProgressParser mod created by @DMagic.
I have included a current version of this in the download.

For those who wish to record your efforts, the Kaptain's Log is for you.

This mod will automatically record events as they happen.  For many of them, an optional window can open to allow you to enter your own notes.

Functionality

	Automatically record events as they happen
	Automatic screenshot at time of event, savable as either JPG or PNG format, or both
	Configurable thumbnail size of screenshots
	Display optional window during event record for personal notes
	Manual entry of logs at any time
	Optional overriding of Escape menu when in flight screen
	Display of log entries on screen
	Selection of which fields to display
	Ability to filter on any of the fields
	Export to CSV
	Export to HTML using template files
	Attach selected image to a manual log entry

Events Recorded

	Flight Log Recorded
	Part Died
	Launch
	Stage Separation
	Part Couple (dock)
	Vessel Modified
	Stage Activate
	Orbit Closed (enter orbit, not from a launch)
	Orbit Escaped (achieve escape velocity)
	Vessel Recovered
	Landed
	Crew Modified
	Progress Record
	Kerbal Passed Out From Gee Force
	Crew Killed
	Crew Transferred
	Dominant Body Change (SOI change)
	Flag Plant
	Crew On EVA
	Manual Entry

Data items being captured are:

	Vessel name (or Kerbal name)
	Universal Time
	UTC time
	Mission Time
	Situation
	Body (which SOI is the vessel in)
	Control level
	Altitude
	Speed
	Event type
	Notes (entered by player)
	Screenshot (both full size and thumbnail)

The HTML template files are in a simple format (not yet finalized).  Essentially, for each template, there will be three files:

	Header file
	Detail line file
	Footer file

I am writing the following simple templates for the initial release:

	Detailed with Full Size Images
	Detailed with Thumbnails
	Details, No images



	My thanks to @DMagic for giving me some icons to use for the message window