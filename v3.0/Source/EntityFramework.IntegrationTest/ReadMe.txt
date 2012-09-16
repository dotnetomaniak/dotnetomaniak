Kigg.Infrastructure.EF.IntegrationTest will require test database with sample data.
The KiGG.Sample.IntegrationTest.sql file is a script that will setup test database with sample
data used for this integration test.
Create your database and run this script KiGG.Sample.IntegrationTest.sql on it.

You'll need to change connection string which is defined in BaseIntegrationFixture.cs
as hardcoded string (will be refactored soon to read from config file). The string you need
to update is _providerConnString at line 12 of BaseIntegrationFixture.cs file.

Test is using System.Transaction.TransactionScope maintain database state. You should know
that you have to enable MS DTS before runing this test as System.Transaction.TransactionScope
will demand it. Otherwise the test will just fail peacefully.

To enable DTC you can simply only services console under administrative tools.
Find Distributed Transaction Coordinator. Make sure it is started, if not then start it manually.


--------DTC for Widnows Vista-------- I think this optional
For Vista users if you wish to enable MS DTS do the following
Start->Run->comexp.msc
Under Component Services Node locate
Component Services->Computers->My Computer->Distributed Transaction Coordinator->Local DTC
Right click on Local DTC
Navigate to Security Tab make sure:
Network DTC Access is checked
Client and Administration checkboxes unchecked
Transaction Manager Communication
Inbound & Outbound checked with Matual Authentication option selected
Click Apply and then OK.


