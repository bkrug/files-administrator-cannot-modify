## One Drive support

Sometimes the files that are constantly processing seem to be the result of the admin user not having modify rights to those files.
This program creates a list of all files in the One Drive folder that the administrator does not have rights to.


See this link:
https://www.technewstoday.com/you-must-have-read-permissions-to-view-the-properties-of-this-object/
for information on changing file permissions.

# Take Ownership and Change Permissions Using CLI

If you find it troublesome to execute the above method, you can use the Windows CLIs like Command Prompt to perform a similar process. It only requires two commands so it’s more convenient. The necessary steps are as follows:

- Press Win + R to open Run.
- Type cmd and press Ctrl + Shift + Enter to open the Elevated Command Prompt.
- Enter the following commands while replacing “Folder With Error” with the problematic file’s full location path:
```
takeown /f “Folder With Error” /a /r /d y
icacls “Folder With Error” /t /c /grant Administrators:F System:F
```