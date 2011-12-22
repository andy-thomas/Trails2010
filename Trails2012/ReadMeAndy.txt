Hi Andy,

This solution uses the following:
ASP.Net MVC3
MEF
MEFContrib for MVC3
SQL Server
JQuery
Modernizr
EntityFramework Code First
NHibernate 3 (includes Linq to NHibernate)
Fluent NHibernate (with both fluent mapping and automapping)


The web project has MEF Contrib, which ties the MEF dependency Injector into the MVC framework. 
By using this, the project only needs to reference the IRepository in Trails2010.DataAccess folder. The controllers are configured to import any objects that MEF gives them. 
By convention, MEF will import any classes which implement the IRepository interface which it finds in any assemby DLLs which it finds lying around in the bin directory.
Thus, the Entity framework repository can be plugged in simply by dropping the "Trails2010.DataAccess.EF" DLL file into the bin folder - no explicit reference is needed. 
(Although, if we _were_ to reference the Trails2010.DataAccess.EF project within the web project, then of course the current up-to-date version would get copied in 
on every build for us, which is more convenient, but leads to tighter coupling). This way, we could simply replace the EF repository with another, say one based on NHibernate, 
simply by swapping the DLLs out.

The main web app and the Trails2012.Tests use MEF plug ins for the implementation of IRepository. The plugins are bound in using the MEF DirectoryCatalog. This points to a "PlugIns" 
folder underneath the respective "bin" folders. (Don't forget to create these if you're creating a new environment!) This folder contains the DLL which implements the required 
exported plugin classes (in this EFRepository or NHibRepository) and all the required dependencies. The executing assembly finds the dependencies using the <probing> element 
in the app.config and web.config files. I do it this way, rather than putting the plug in DLLS directly in bin folders, so that the DLLs do not get deleted during rebuilds.


Links for uploading files:
http://stackoverflow.com/questions/4784225/mvc-3-file-upload-and-model-binding  <----
http://aspzone.com/tech/jquery-file-upload-in-asp-net-mvc-without-using-flash/
http://haacked.com/archive/2010/07/16/uploading-files-with-aspnetmvc.aspx
http://www.hanselman.com/blog/ABackToBasicsCaseStudyImplementingHTTPFileUploadWithASPNETMVCIncludingTestsAndMocks.aspx
Links for dealing with images
http://stackoverflow.com/questions/4896439/action-image-mvc3-razor
