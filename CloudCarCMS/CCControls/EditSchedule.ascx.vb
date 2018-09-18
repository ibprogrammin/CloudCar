Public Partial Class EditSchedule
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Property Day() As DayOfWeek
        Get
            Return hfDay.Value
        End Get
        Set(ByVal value As DayOfWeek)
            hfDay.Value = value
        End Set
    End Property

    Public Property ProgramID() As Integer
        Get
            Return hfProgramID.Value
        End Get
        Set(ByVal value As Integer)
            hfProgramID.Value = value
        End Set
    End Property

    Public Sub CheckIfScheduleExists()
        LoadSchedule(ProgramID, Day)
    End Sub

    Public Property ScheduleID() As Integer
        Get
            Return hfScheduleID.Value
        End Get
        Set(ByVal value As Integer)
            hfScheduleID.Value = value
            LoadSchedule(value)
        End Set
    End Property

    Public Sub Show()
        mpeEditSchedule.Show()
    End Sub

    Private Sub LoadSchedule(ByVal ScheduleID As Integer)
        Dim schedule As Schedule

        schedule = New ScheduleModule.ScheduleController().GetElement(ScheduleID)
        If Not schedule Is Nothing Then
            'txtHours.Text = schedule.Hours
            ProgramID = schedule.ProgramID
            'Day = schedule.Day
        End If

        schedule = Nothing
    End Sub

    Private Sub LoadSchedule(ByVal ProgramID As Integer, ByVal Day As DayOfWeek)
        ScheduleID = cProgram.GetScheduleID(ProgramID, Day)
        LoadSchedule(ScheduleID)
    End Sub

    Public Event ScheduleSaved(ByVal Day As DayOfWeek)
    Public Event ScheduleDeleted(ByVal Day As DayOfWeek)

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ScheduleID = cSchedule.SaveSchedule(ScheduleID, ProgramID, Day, txtHours.Text)

        cSchedule.GeneratePDF(Server.MapPath("/Reports/Schedule.rdlc"), Server.MapPath("/Reports/Schedule.pdf"))

        RaiseEvent ScheduleSaved(Day)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        cSchedule.DeleteSchedule(ScheduleID)

        RaiseEvent ScheduleDeleted(Day)
    End Sub

End Class