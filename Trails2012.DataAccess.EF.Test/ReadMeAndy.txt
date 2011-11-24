Hi Andy

The connection string is set up to go against the main database: (connectionString="Data Source=E6500-ANDYT\SQL2008;Initial Catalog=Trails;Integrated Security=True"). 
This database does not have a EdmMetadata table, so Entity Framework does not use it to track changes to the domain model POCOs and/or mapping configurations.

However, if you use the other connection string listed in the App.Config file ("Data Source=.\SQLExpress;Initial Catalog=Trails2012.DataAccess.EF.TrailsContext;Integrated Security=True")
or if you delete/comment all connection strings, then Entity Framework will auto-generate a new one using the connection string listed above (these are it's default settings).

When autogenerating the database, EF will use the TrailsInitializer class, since it was configured in the ClassInitialize method in each of the test classes.

The TrailsInitializer class seeds the daa with known data, which is used by the automated intergration tests.

So, if you do not have a database set up, but do have SQL Express installed on your machine, then just run one of the tests. EF will autogenerate and seed the database. 
You can then use this database in the main web application (no need to set a connection string in web.config - EF will go to it by default)
