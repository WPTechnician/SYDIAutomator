# SYDIAutomator
SYDIAutomated

A utility for automating the documentation of servers.

Instructions

SYDI Portion

1) Pick a system that is in the same domain as the OUs in your AD where your server objects live.
2) Copy the scripts folder to a local drive on that server (C:\scripts).
3) Edit the SYDIautomated.exe.config file in the scripts folder and update the settings as follows:
	a) <setting name="OU" 		- the DN of the OU where your servers live.
	b) <setting name="DCOU"		- the DN of the OU where your domain controllers live.
	c) <setting name="Web_Path" 	- the path to the web for the resulting files to be read by the web front end

4) On the WEB server, create a folder in the root called "SYDI-Docs" and a folder under that called files.
5) Once everything is confirmed and saved, double-click sydiautomated.exe in the scripts folder.  If everything is configured properly, a set of html files corresponding to your servers will appear in the path you created on the web server.

Web Portion

1) On the web server, create a new website using the web folder contents
2) Ensure the app pool is set for .NET 4.0
3) Ensure that the IIS_Users group has write access to the temp directory in that web site
4) Edit the web.config in notepad and update the settings as specified by the instructions in the file.
5) If there are no files you won't see anything so make sure you complete the SYDI portion first.

Scheduling

To schedule this, simply create a new task on the server where you performed the first step and point the task at the sydiautomated.exe file.  Make sure it runs in the context of someone who has read/write access to all the file paths.
