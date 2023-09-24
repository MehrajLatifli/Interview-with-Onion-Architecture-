<img src="https://github.com/MehrajLatifli/Interview-with-Onion-Architecture-/blob/master/Assets/image.png" alt="Your Image Description" width="500">


<h1> SQL </h1>
<a href="https://github.com/MehrajLatifli/Interview-with-Onion-Architecture-/blob/master/Assets/Interview.sql">Interview SQL file</a>
</br>
<img src="https://github.com/MehrajLatifli/Interview-with-Onion-Architecture-/blob/master/Assets/SQL%20Diagram.png" alt="Your Image Description" width="500">

</br>

<h1> Migration </h1>
add-migration AnyMigration -Context Interview.Persistence.Contexts.AuthDbContext.DbContext.CustomDbContext 
</br>
update-database -Context Interview.Persistence.Contexts.AuthDbContext.DbContext.CustomDbContext 

</br>

<h1> MSSQL Server </h1>
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=admin1234@" -p 1430:1433 --name sql2 --hostname sql2 -d ` mcr.microsoft.com/mssql/server:2022-latest 

</br>

<h1> Azure Blob Storage </h1>

docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 --name some-azurite mcr.microsoft.com/azure-storage/azurite

Microsoft Azure Storage Explorer -> Local Storage Emulator (Display name: Interview)

Connection string:   AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1; 