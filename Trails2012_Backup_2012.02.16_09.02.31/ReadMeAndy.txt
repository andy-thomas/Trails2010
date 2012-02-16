Hi Andy,

This solution uses the following:
ASP.Net MVC3
MVC Scaffolding
MEF
MEFContrib for MVC3
SQL Server
JQuery
JQuery UI
Telerik MVC Extensions
Modernizr
Elmah
Elmah.SqlServerCompact
EntityFramework Code First
NHibernate 3 (includes Linq to NHibernate)
Fluent NHibernate (this project utilises both fluent mapping and automapping)
(NHibernate uses Castle Core and Iesi Collections, MEF uses WebActivator, MVC Scaffolding uses T4Scaffolding)

The web project has MEF Contrib, which ties the MEF dependency Injector into the MVC framework. 
By using this, the project only needs to reference the IRepository in Trails2010.DataAccess folder. The controllers are configured to import any objects that MEF gives them. 
By convention, MEF will import any classes which implement the IRepository interface which it finds in any assemby DLLs which it finds lying around in the bin directory 
(or, in this case, the "bin\PlugIns" directory - see below).
Thus, the Entity framework repository can be plugged in simply by dropping the "Trails2010.DataAccess.EF" DLL file into the bin folder - no explicit reference is needed. 
(Although, if we _were_ to reference the Trails2010.DataAccess.EF project within the web project, then of course the current up-to-date version would get copied in 
on every build for us, which is more convenient, but leads to tighter coupling). This way, we could simply replace the EF repository with another, say one based on NHibernate, 
simply by swapping the DLLs out.

The main web app and the Trails2012.Tests use MEF plug ins for the implementation of IRepository. The plugins are bound in using the MEF DirectoryCatalog. This points to a "PlugIns" 
folder underneath the respective "bin" folders. (Don't forget to create these if you're creating a new environment!) This folder contains the DLL which implements the required 
exported plugin classes (in this EFRepository or NHibRepository) and all the required dependencies. The executing assembly finds the dependencies using the <probing> element 
in the app.config and web.config files. I do it this way, rather than putting the plug in DLLs directly in bin folders, so that they do not get deleted during rebuilds.

Error handling is logged using Elmah, with a SQL Server Compact store for the error messages. The database itself is kept in the App_Data folder, in the Elmah.sdf file. 
It can viewed through the Visual Studio Server Explorer. I have added a custom ErrorHandlerAttribute class (written by Atif Aziz, creator of Elmah), in order to pass 
handled exceptions through to elmah. This is set up in the RegisterGlobalFilters method in Global.asax. I have also added a customErrors entry in Web.config with a 
default redirect url = "~/error". In order for this url to work, I created an ErrorController which simply displays the shared Error.cshtml view. 
I have added a link on this shared Error view to navigate the users to elmah.axd, where they can view the detailed errors 
(although, in practice, this page should only be available to admin/support users - see http://code.google.com/p/elmah/wiki/SecuringErrorLogPages).
(If you want a laugh, see "11 easy steps" to setting up "404" error handling in MVC - http://secretgeek.net/custom_errors_mvc.asp). 
Also see the following for a more balanced summary:
http://community.codesmithtools.com/CodeSmith_Community/b/tdupont/archive/2011/03/01/error-handling-and-customerrors-and-mvc3-oh-my.aspx.)


Links for uploading files:
http://stackoverflow.com/questions/4784225/mvc-3-file-upload-and-model-binding  <----
http://aspzone.com/tech/jquery-file-upload-in-asp-net-mvc-without-using-flash/
http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx
http://www.hanselman.com/blog/ABackToBasicsCaseStudyImplementingHTTPFileUploadWithASPNETMVCIncludingTestsAndMocks.aspx
Links for dealing with images
http://stackoverflow.com/questions/4896439/action-image-mvc3-razor
