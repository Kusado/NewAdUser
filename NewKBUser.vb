Private Sub bUserAdmin_Click(sender As System.Object, e As System.EventArgs) Handles bUserAdmin.Click
  Try
    If txtDBLogin.Text = "" Or txtDBLastName.Text = "" Then
      MsgBox("Для заведения пользователя должны быть заполнены логин и фамилия.", MsgBoxStyle.Critical, "Ошибка")
      Return
    End If

    Dim Login As String = txtDBLogin.Text
    Dim ErrText As String = ""
    'Создание логина на сервере
    Dim SQL As New SQL(Me.DBConnectionString, "KB")
    SQL.ExecuteNonQuery("IF NOT EXISTS(SELECT loginname FROM sys.syslogins WHERE loginname = '" & Login & "') EXEC sp_grantlogin '" & Login & "'", CommandType.Text, ErrText, False)
    If ErrText IsNot Nothing Then Throw New Exception(ErrText)

    'Создание пользователя в БД KB
    SQL.ExecuteNonQuery("IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = '" & Login & "')" & vbCrLf &
        "CREATE USER [" & Login & "] FOR LOGIN [" & Login & "]", CommandType.Text, ErrText, False)
    If ErrText IsNot Nothing Then Throw New Exception(ErrText)

    'Заведение пользователя в таблице

    SQL = New SQL(Me.DBConnectionString, "KB")
    Dim sMenu As String = ""
    If rbDBUser.Checked = False Then
      sMenu = "INSERT INTO UserMenu (IDUser, IDMenu, Show) VALUES (@IDUser, '7d1037f0-89fc-45b0-bfa6-e288e471a718', 1);" & vbCrLf
    End If
    SQL.ExecuteNonQuery("IF NOT EXISTS(SELECT 1 FROM Logins WHERE Login = '" & Login & "') BEGIN" & vbCrLf &
        "DECLARE @IDUser int;" & vbCrLf &
        "INSERT INTO Users (LastName, FirstName, MiddleName, EMail) VALUES ('" & txtDBLastName.Text & "', '" & txtDBFirstName.Text & "', '" & txtDBMiddleName.Text & "', " & If(txtDBEMail.Text = "", "NULL", "'" & txtDBEMail.Text & "'") & ");" & vbCrLf &
        "SET @IDUser = SCOPE_IDENTITY();" & vbCrLf &
        sMenu &
        "INSERT INTO Logins (IDUser, Login) VALUES (@IDUser, '" & Login & "')" & vbCrLf &
        "END", CommandType.Text, ErrText, False)
    If ErrText IsNot Nothing Then Throw New Exception(ErrText)

    'Роль IT в БД KB
    If rbDBUser.Checked = False Then
      SQL = New SQL(Me.DBConnectionString, "KB")
      SQL.ExecuteNonQuery("EXEC sp_addrolemember 'IT', '" & Login & "'", CommandType.Text, ErrText, False)
      If ErrText IsNot Nothing Then Throw New Exception(ErrText)
    End If
    If rbDBAdmin.Checked Then
      SQL = New SQL(Me.DBConnectionString)
      SQL.ExecuteNonQuery("EXEC sp_addsrvrolemember '" & Login & "', 'sysadmin'", CommandType.Text, ErrText, False)
      If ErrText IsNot Nothing Then Throw New Exception(ErrText)
    End If

    MsgBox("Операция успешно выполнена.", MsgBoxStyle.Information, "Заведение пользователя")
  Catch ex As Exception
    MsgBox("Ошибка заведения пользователя." & vbCrLf & ex.Message, MsgBoxStyle.Critical, "Ошибка")
  End Try
End Sub