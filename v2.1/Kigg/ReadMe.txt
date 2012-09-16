Version 2.1
============

Clean Install
-------------
1. Extract the Zip in a Directory (e.g. C:\Projects\KiGG)
2. Create Web Folder named KiGG (e.g. C:\Projects\KiGG\Web) as Application in IIS7 manager. Ensure that application pool is set to Default. (Skip this step, in case you are using Integrated Web Server of Visual Studio)
3. Now Open and execute the Create.sql script from Database Folder (e.g. C:\Projects\KiGG\Database\Create.sql) and run in SQL Management Studio.
4. Next Open and execute the Data.sql from the Database folder (e.g. C:\Projects\KiGG\Database\data.sql) to populate some tag/category.
5. Now Open the Kigg.sln in Visual Studio.
6. Change the Database Connection String.
7. Now Create an Account in http://www.pageglimpse.com/ (Currently free service) and put it in web.config line 567.
8. Now Create an Account in http://recaptcha.net (free service) to get your API Keys. Once you got those put in your web.config line 1190 and 1193.
9. Right Click the Web Project and click Properties, then select the Web Tab. Now Check the Use Local IIS Web Server checkbox and set http://localhost/KiGG as Project Url. (Skip this step, in case you are using Integrated Web Server of Visual Studio)
10. Now Run, you will be able to see the Home page with "No published story exists." message.

Additional Notes
-----------------
1. To show the Vote Counter it is required to know the vistual path. Open the web.config and put your virtual path in line 301 (e.g. http://localhost/KiGG). In case of Intgrated Visual Studio Web Server specify it with the port number.
2. Open the Web.config and try find with "YOUR-" and replace with your value.
3. For external spam checking service create account and replace the api key in web.config with your value (Check the Web.full.config)
	-http://akismet.com/
	-http://defensio.com/
	-http://antispam.typepad.com/
4. You can also use websnapr.com for the Thumbnail instead of PageGlimpse. Just create an account in http://www.websnapr.com/ and repalce with your value.


Upgrading from 2.0
------------------
1. Backup  your exisiting code base.
2. Run the Upgrade-From-2.0.sql from Database folder (e.g. C:\Projects\KiGG\Database\Upgrade-From-2.0.sql) it will drop the version column which is no longer required.
