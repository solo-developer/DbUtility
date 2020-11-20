# OracleDbUtility
Helper classes to perform database operations on Oracle DB

##### 1. DatabaseUtility.cs
 Helps backing up database into dump file

##### 2. StoredProcedureHelper.cs (SQL Server)
Helps in executing stored procedure which has custom table types as parameters . Works with simple stored procedure too.

###### Usage :
    var param = new List<StoredProcedureRequestModel>()
                {
                   new StoredProcedureRequestModel() { ParamName = "@ParamName", Value = value }
                };
    List<ResponseModel> data = StoredProcedureHelper<DbContext>.ExecuteStoredProcedure<ResponseModel>("stored procedure name", BasicsParam);

and with complex type :

    var complexParam = new List<StoredProcedureRequestModel>()
                {
                    new StoredProcedureRequestModel(){ ParamName="@param name",IsCustomType=true,CustomTableTypeName="table type name",CustomTypeColumnName="column name of table type",CustomTypeValues=paramValuesToPass}
                };
    List<ResponseModel> data = StoredProcedureHelper<DbContext>.ExecuteStoredProcedure<ResponseModel>("stored procedure name", complexParam);

