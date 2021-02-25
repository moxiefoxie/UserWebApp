# UserWebApp
for application Domain

To Start -
You will have to restore Nuget Packages
Open Package Manager Console and run 'Update-Database'
If you get an error such as "part of file not found", clean and build solution before running. 
In addition to this, the*.mdf file containing the database can be found under the App_Data folder when viewing in File Explorer.
Cut the .mdf and .ldf files from the main project folder and paste them in the App_Data folder. You will replace a file already there.
Open Package Manager Console from View -> More WIndows and run the folloring command: Update-Package Microsoft.CodeDom.Providers.DotNetCompilerPlatform -r
