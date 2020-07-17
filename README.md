# SYDIAutomator
SYDIAutomated

A utility for automating the documentation of servers.

Instructions

SYDI Portion

1) Pick a system that is in the same domain as the OUs in your AD where your server objects live.
2) Unzip the scripts.zip file into a folder on the C drive of that server called scripts (c:\scripts)
3) Edit the SYDIautomated.exe.config file and update the settings as follows:
	a) <setting name="OU" 		- the DN of the OU where your servers live.
	b) <setting name="DCOU"		- the DN of the OU where your domain controllers live.
	c) <setting name="FTP_Address" 	- the IP address of the FTP server on the web server you are going to use
	d) <setting name="FTP_UserName" - the FTP server username
	e) <setting name="FTP_Password" - the FTP server password

4) On the FTP/WEB server, create a folder in the root called "SYDI-Docs" and a folder under that called files.
5) Once everything is confirmed and saved, double-click c:\scripts\sydiautomated.exe.  if everything is configured properly, a set of htm files corresponding to your servers will appear on the FTP server.

Web Portion

1) On the web server, create a new website using the web.zip contents
2) Ensure the app pool is set for .NET 4.0
3) Ensure that the IIS_Users group has write access to the temp directory in that web site
4) Edit the web.config in notepad and update the settings as specified by the instructions in the file.
5) If there are no files you won't see anything so make sure you complete the SYDI portion first.

Scheduling

To schedule this, simply create a new task on the server where you performed the first step and point the task at the sydiautomated.exe file.  Make sure it runs in the context of someone who has read/write access to all the file paths.

Note: In my environment I have the Web and FTP services running on the same server which makes everything a lot easier.
