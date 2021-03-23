namespace FSharpWPF.Commands

module helpers 
    open System
    open System.Data

    type Column = { Name:String; Type:DbType }
    type Values = { ColumnName:String; Value:obj }
    type Query = { Type:StatementType; TableName:String; Columns: Values list; IdColumn:string }

    let SELECT_TEMPLATE:FormattableString = $"SELECT {0} FROM {1}"; 
    let INSERT_TEMPLATE:FormattableString = $"INSERT INTO {0} ({1}) VALUES ({2})";
    let UPDATE_TEMPLATE:FormattableString = $"UPDATE {0} SET {1} WHERE {2}=@{2}";
    let DELETE_TEMPLATE:FormattableString = $"DELETE FROM {0} WHERE {1}";

    let formatSelect (query: Query) =
        let formattedCollumns = 
            query.Columns
            |> Seq.map(fun c -> c.ColumnName)
            |> Seq.toArray
            |> fun s -> String.Join(",", s) 
        String.Format(SELECT_TEMPLATE.Format, query.TableName, formattedCollumns)

    let formatUpdate (query: Query) =
        let setExpression = 
            query.Columns
            |> Seq.map(fun c -> $"{c.ColumnName}=@{c.ColumnName}") 
            |> Seq.toArray
            |> fun s -> String.Join(",", s) 
        String.Format(UPDATE_TEMPLATE.Format, query.TableName, setExpression, query.IdColumn, query.IdColumn)

    let formatInsert (query: Query) =
        let collumnsExpression = 
            query.Columns
            |> Seq.map(fun c -> $"{c.ColumnName}") 
            |> Seq.toArray
            |> fun s -> String.Join(",", s) 
        let ValuesExpression = 
            query.Columns 
            |> Seq.map(fun c -> $"@{c.ColumnName}")
            |> Seq.toArray
            |> fun s -> String.Join(",", s)
        String.Format(INSERT_TEMPLATE.Format, query.TableName, collumnsExpression, ValuesExpression)

    let formatDelete (query: Query) =
        String.Format(DELETE_TEMPLATE.Format, query.TableName, $"{query.IdColumn}=@{query.IdColumn}")

    let (|Select|) (query:Query) =
        formatSelect query

    let (|Insert|) (query:Query) =
        formatInsert query

    let (|Update|) (query:Query) =
        formatUpdate query
    
    let (|Delete|) (query:Query) =
        formatDelete query

    let (|Select|Insert|Update|Delete|) (query) =
        match query.Type with
        | StatementType.Select -> Select //query
        | StatementType.Insert -> Insert //query
        | StatementType.Update -> Update //query
        | StatementType.Delete -> Delete //query

    let getColumnsFromQuery (query:Query) =
        query.Columns
        |> Seq.map(fun c -> c.ColumnName)
        |> Seq.toArray


    let getCommand (query:Query) (commandText:String):MySqlCommand = 
        let command = new MySqlCommand(commandText)     
        getColumnsFromQuery query
        |> fun arr -> 
            if not (arr |> Array.contains query.IdColumn) then 
                Array.append arr [|query.IdColumn|]
            else arr
        |> Array.map(fun c -> new MySqlParameter(c, null))
        |> command.Parameters.AddRange
        command

    let printCommandText (command:IDbCommand) =
        printfn "%s" command.CommandText

    let printCommandParameters (command:IDbCommand) =
        ArrayList.Adapter(command.Parameters).ToArray()
        |> Array.map(fun p -> p :?> IDbDataParameter)
        |> Array.iter(fun p -> printfn "%s" p.ParameterName)

    let getCommandParameters (command:MySqlCommand) =
        ArrayList.Adapter(command.Parameters).ToArray()
        |> Array.map(fun p -> p :?> MySqlParameter)



    let setParametersValues (query:Query) (parameters:MySqlParameter[]) = 
        parameters 
        |> Array.iter(fun parameter ->
            query.Columns
            |> List.find(fun c -> c.ColumnName = parameter.ParameterName)
            |> fun c -> parameter.Value <- c.Value)                

    let matchQuery query = 
        match query with 
        | Select -> (|Select|)(query) |> getCommand query
        | Insert -> (|Insert|)(query) |> getCommand query
        | Update -> (|Update|)(query) |> getCommand query
        | Delete -> (|Delete|)(query) |> getCommand query
    
    let executeCommands queries connectionString =
        let mutable result = 0
        using(new MySqlConnection(connectionString))
            (fun connection -> 
                connection.Open()
                for query in queries do
                    matchQuery query
                    |> fun command -> 
                        getCommandParameters command
                        |> setParametersValues query

                        command.Connection <- connection
                        result <- command.ExecuteNonQuery()
                connection.Close())
        result
