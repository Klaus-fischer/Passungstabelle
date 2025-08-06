﻿Imports SolidWorks.Interop.sldworks
Imports SolidWorks.Interop.swconst
Imports System.Collections.Generic

Public Class Passungstabelle_Datei
    Property Blätter As Passungstabelle_Blatt()
    Property BlätterStr As String()

    Property Attr_generell As New Dictionary(Of String, String)
    Property Attr_Übersetzungen As New Dictionary(Of String, Dictionary(Of String, String))
    Property Attr_Formate As New Dictionary(Of String, Dictionary(Of String, String))
    Property Attr_Tabelle As New Dictionary(Of String, Dictionary(Of String, String))
    '2022-06-24
    'Property Attr_Meldungen As New Dictionary(Of String, Dictionary(Of String, Boolean))

    'Property Log As New LogFile(Attr_generell)
    Property Log As LogFile

    Property Swapp As SldWorks
    Property SwDraw As DrawingDoc

    '** Speichert das aktive Blatt
    Private Property Swcsheet As SolidWorks.Interop.sldworks.Sheet

    Sub New(iSwapp As SldWorks)
        Swapp = iSwapp
    End Sub

    ' Erstellt eine Liste der Blätter in der Datei
    Function PassungsTabelleGetSheets(swmodel As SolidWorks.Interop.sldworks.ModelDoc2) As Boolean
        Dim ok As Boolean

        ok = False
        SwDraw = swmodel

        Log.WriteInfo(swmodel.GetPathName, "", False)

        '* Namen der Blätter speichern
        BlätterStr = swDraw.GetSheetNames()
        If BlätterStr.Length = 0 Then
            Log.WriteInfo(My.Resources._Keine_Blätter_in_der_Zeichnung, "", True)
            PassungsTabelleGetSheets = ok
            Exit Function
        End If

        '* Aktuelles Blatt speichern um diese später wieder zu aktivieren
        Swcsheet = SwDraw.GetCurrentSheet

        'Blätter suchen und initialisieren
        SetBlätter(swmodel)

        SwDraw.ActivateSheet(Swcsheet.GetName)
        PassungsTabelleGetSheets = True
    End Function

    Sub SetBlätter(swmodel As ModelDoc2)
        Dim i As Integer
        Dim z As Integer

        'Wenn nur die Tabelle nur auf dem ersten Blatt erscheinen soll und 
        'auf den restlichen Blättern nicht gelöscht werden soll
        'brauchen wir nur das erste Blatt untersuchen, sonst müssen auch die 
        'restlichen Blätter durchsucht werden ob eine alte Tabelle vorhanden ist
        If Attr_generell("NurAufErstemBlatt") And Attr_generell("LöschenAufRestlichenBlättern") = False Then
            z = 0
            ReDim Blätter(i)
        Else
            z = UBound(BlätterStr)
            ReDim Blätter(z)
        End If

        For i = 0 To z
            Blätter(i) = New Passungstabelle_Blatt(Swapp, Attr_generell, Attr_Übersetzungen, SwDraw.Sheet(BlätterStr(i)), swmodel, Log)
            Blätter(i).SetSheetAttr(Attr_Formate, Attr_Tabelle)
            '* Ermittlung der Ansichten auf dem Blatt
            Blätter(i).PassungsTabelleGetViews(swmodel, BlätterStr(i))
            'Blätter(i).Log = Log
        Next

        'suchen nach Passungen auf den Blättern
        PassungstabelleGetdimensionFromSheets()
    End Sub

    'Löscht die Tabellen auf allen Blättern außer dem ersten
    Sub DelTableOnOtherSheets()
        Dim sh As Sheet
        sh = SwDraw.GetCurrentSheet

        For i = 1 To BlätterStr.Length - 1
            'sh = swDraw.Sheet(BlätterStr(i))
            SwDraw.ActivateSheet(Blätter(i).Blatt.GetName)
            Blätter(i).DeleteTab()
        Next

        SwDraw.ActivateSheet(sh.GetName)
    End Sub

    'Löscht die Tabellen auf allen Blättern 
    Sub DelAllTables()
        Dim sh As Sheet
        sh = SwDraw.GetCurrentSheet

        For i = 0 To BlätterStr.Length - 1
            'sh = SwDraw.Sheet(BlätterStr(i))
            SwDraw.ActivateSheet(Blätter(i).Blatt.GetName)
            Blätter(i).DeleteTab()
        Next
        SwDraw.ActivateSheet(sh.GetName)
    End Sub

    Function PassungstabelleGetdimensionFromSheets() As Boolean
        Dim i As Integer = 0
        Dim z As Integer = 0

        'Wenn Tabelle nur auf dem ersten Blatt erscheinen soll,
        'dann wird auch nur auf dem ersten Blatt nach Passungen gesucht
        If Attr_generell("NurAufErstemBlatt") Then z = 0 Else z = Blätter.Length - 1

        For i = 0 To z
            Blätter(i).SetEinfügepunkt()
            Blätter(i).PassungsTabelleGetDimensions()
            Blätter(i).SetEinfügePunktPosition()
        Next

        PassungstabelleGetdimensionFromSheets = True
    End Function

    Sub InsertTableOnSheets()
        Dim PassungenGefunden As Boolean = False
        Dim swsheet As Sheet

        swsheet = SwDraw.GetCurrentSheet

        'Prüfung ob auf zumindest einem Blatt, Passungen gefunden wurden
        For Each blatt In Blätter
            If Not blatt.Tabelle Is Nothing Then
                If blatt.Tabelle.TabellenZeilen.Count > 0 Then
                    PassungenGefunden = True
                    'Check ob auf einem der Blätter ev. eine Tabelle vorhanden ist
                    'Wenn ja, dann wird die Tabelle gelöscht
                ElseIf Not blatt.AlteTabelle Is Nothing Then
                    SwDraw.ActivateSheet(blatt.Blatt.GetName)
                    blatt.DeleteTab()
                End If
            End If
        Next

        SwDraw.ActivateSheet(swsheet.GetName)

        'Keine Passungen gefunden
        If Not PassungenGefunden Then
            'Wenn Fehlermeldung ausgegeben werden soll
            Log.WriteInfo(My.Resources._Keine_Passungen_gefunden, "", True)
            Exit Sub
        End If


        'Wenn die Tabelle auf den restlichen Blättern gelöscht werden soll
        If Attr_generell("NurAufErstemBlatt") = True And Attr_generell("LöschenAufRestlichenBlättern") = True Then
            DelTableOnOtherSheets()
            Blätter(0).InsertTable()
        End If
        If Attr_generell("NurAufErstemBlatt") = False And Attr_generell("LöschenAufRestlichenBlättern") = False Then
            For Each n In Blätter
                n.InsertTable()
            Next
        End If
        If Attr_generell("NurAufErstemBlatt") = False And Attr_generell("LöschenAufRestlichenBlättern") = True Then
            DelAllTables()
        End If
        If Attr_generell("NurAufErstemBlatt") = True And Attr_generell("LöschenAufRestlichenBlättern") = False Then
            Blätter(0).InsertTable()
        End If

        SwDraw.ActivateSheet(Swcsheet.GetName)
    End Sub

End Class

