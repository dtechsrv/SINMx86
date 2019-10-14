Imports System
Imports System.Management
Imports System.Math
Imports System.Convert
Imports Microsoft.Win32

Imports SINMx86.Localization

' Processzor információs ablak osztálya
Public Class IPInfo

    ' WMI feldolgozási objektumok
    Public objNC As ManagementObjectSearcher
    Public objMgmt As ManagementObject

    ' *** FŐ ELJÁRÁS: IP-infó ablak betöltése (MyBase.Load -> IPInfo) ***
    ' Eseményvezérelt: Ablak megnyitása
    Private Sub IPInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Értékek definiálása
        Dim AdapterName As String = Nothing                     ' Hálózati adapter neve
        Dim DHCPClient As Boolean = False                       ' DHCP kliens
        Dim IPEnabled As Boolean = False                        ' IP kapcsolat engedélyezve
        Dim IPAddress() As String                               ' IP-címek tömbje
        Dim Netmask() As String                                 ' Alhálózati maszkok tömbje
        Dim LeaseStart, LeaseEnd As DateTime                    ' DHCP címkiosztási időparaméterek
        Dim IPCount As Int32                                    ' IP-cím sorszáma
        Dim Gateway() As String                                 ' Átjárók tömbje
        Dim Metric() As Int16                                   ' Átjáró metrika értékek tömbje
        Dim GWCount As Int32                                    ' Átjáró sorszáma
        Dim DNSServer() As String                               ' DNS kiszolgálók tömbje
        Dim DNSCount As Int32                                   ' DNS sorszáma
        Dim Domain As String = Nothing                          ' DNS domain név
        Dim NetBIOS As Int32 = 0                                ' NetBIOS TCP/IP felett

        ' Értékek átvétele a főablaktól
        Dim SelectedInterface = MainWindow.SelectedInterface
        Dim InterfaceName As String = MainWindow.ComboBox_InterfaceList.Items(SelectedInterface)
        Dim InterfaceID As String = MainWindow.InterfaceID(SelectedInterface)

        ' Ablak nevének  átvétele a főablakból
        Me.Text = GetLoc("IPTitle")

        ' Billentyűk figyelése
        Me.KeyPreview = True

        ' GroupBox szövegének beállítása
        GroupBox_Table.Text = GetLoc("IPTable") + " " + InterfaceName

        ' Bezárás gomb
        Button_Close.Text = GetLoc("Button_Close")

        ' Tábla fejléc szövegek átvétele a főablakból
        IP_Table.Columns(1).Text = GetLoc("IPDescription")
        IP_Table.Columns(2).Text = GetLoc("IPValue")

        ' Ablak láthatósága
        If MainWindow.TopMost Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If

        ' Sorok törlése
        IP_Table.Items.Clear()

        ' WMI érték definiálása
        objNC = New ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index = '" + InterfaceID + "'")

        ' Értékek kinyerése a WMI-ből
        For Each Me.objMgmt In objNC.Get()

            ' Sorok felvitele
            AdapterName = objMgmt("Description")
            If InStr(AdapterName, " - Packet Scheduler Miniport") Then
                AdapterName = Replace(AdapterName, " - Packet Scheduler Miniport", "")
            End If
            IPTableAddRow(GetLoc("IPAdapter"), AdapterName)
            IPTableAddRow(GetLoc("IPMACAddr"), objMgmt("MACAddress"))

            ' IP kapcsolat elérhetőségének ellenőrzése
            If objMgmt("IPEnabled") Then

                ' DHCP kliens ellenőrzése
                DHCPClient = objMgmt("DHCPEnabled")

                If DHCPClient Then
                    IPTableAddRow(GetLoc("IPDHCPClient"), GetLoc("Enabled"))
                Else
                    IPTableAddRow(GetLoc("IPDHCPClient"), GetLoc("Disabled"))
                End If

                ' IP-címek és netmaszkok lekérdezése
                If Not IsNothing(objMgmt("IPAddress")) Then

                    IPAddress = objMgmt("IPAddress")
                    Netmask = objMgmt("IPSubnet")

                    ' IP-címek számának meghatározása
                    IPCount = UBound(IPAddress)

                    ' IP-címek felvitele (A címke csak az első címnél kerül ki!)
                    For IPCount = 0 To UBound(IPAddress)
                        If IPCount = 0 Then
                            IPTableAddRow(GetLoc("IPAddress"), IPAddress(IPCount) + "/" + GetNetmask(Netmask(IPCount)))
                        Else
                            IPTableAddRow(Nothing, IPAddress(IPCount) + "/" + GetNetmask(Netmask(IPCount)))
                        End If
                    Next
                End If

                ' Átjárók és metrikájuk lekérdezése
                If Not IsNothing(objMgmt("DefaultIPGateway")) Then

                    Gateway = objMgmt("DefaultIPGateway")
                    Metric = objMgmt("GatewayCostMetric")

                    ' Átjárók számának meghatározása
                    GWCount = UBound(Gateway)

                    ' Átjárók felvitele (A címke csak az első címnél kerül ki!)
                    For GWCount = 0 To UBound(Gateway)
                        If GWCount = 0 Then
                            IPTableAddRow(GetLoc("IPGateway"), Gateway(GWCount) + " (" + GetLoc("IPMetric") + ": " + Metric(GWCount).ToString + ")")
                        Else
                            IPTableAddRow(Nothing, Gateway(GWCount) + " (" + GetLoc("IPMetric") + ": " + Metric(GWCount).ToString + ")")
                        End If
                    Next
                End If

                ' DNS kiszolgálók lekérdezése
                If Not IsNothing(objMgmt("DNSServerSearchOrder")) Then

                    DNSServer = objMgmt("DNSServerSearchOrder")

                    ' DNS-ek számának meghatározása
                    DNSCount = UBound(DNSServer)

                    ' DNS-ek felvitele (A címke csak az első címnél kerül ki!)
                    For DNSCount = 0 To UBound(DNSServer)
                        If DNSCount = 0 Then
                            IPTableAddRow(GetLoc("IPDNS"), DNSServer(DNSCount))
                        Else
                            IPTableAddRow(Nothing, DNSServer(DNSCount))
                        End If
                    Next
                End If

                ' DNS domain név ellenőrzése
                Domain = objMgmt("DNSDomain")

                If Domain <> Nothing Then
                    IPTableAddRow(GetLoc("IPDomain"), Domain)
                End If

                ' WINS kiszolgálók lekérdezése
                If Not IsNothing(objMgmt("WINSPrimaryServer")) Then
                    IPTableAddRow(GetLoc("IPWINS"), objMgmt("WINSPrimaryServer"))
                End If

                ' További DHCP paraméterek beállítása
                If DHCPClient Then

                    If objMgmt("DHCPServer") = "255.255.255.255" Then
                        IPTableAddRow(GetLoc("IPDHCPServer"), GetLoc("NotAvailable"))
                    Else
                        IPTableAddRow(GetLoc("IPDHCPServer"), objMgmt("DHCPServer"))
                        LeaseStart = MainWindow.DateTimeConv(objMgmt("DHCPLeaseObtained"))
                        LeaseEnd = MainWindow.DateTimeConv(objMgmt("DHCPLeaseExpires"))

                        ' Bérleti idő ellenőrzése (Win10 hiba miatt!)
                        If DateDiff("s", LeaseStart, DateTime.Now) > 0 And DateDiff("s", LeaseEnd, DateTime.Now) < 0 Then

                            ' DHCP lekérési és lejárati idő hozzáadása
                            IPTableAddRow(GetLoc("IPDHCPStart"), GetLocalizedDate(LeaseStart))
                            IPTableAddRow(GetLoc("IPDHCPEnd"), GetLocalizedDate(LeaseEnd))

                        End If
                    End If
                End If

                ' NetBIOS ellenőrzése
                NetBIOS = objMgmt("TcpipNetbiosOptions")

                If NetBIOS > 1 Then
                    IPTableAddRow(GetLoc("IPNetBIOS"), GetLoc("Disabled"))
                Else
                    IPTableAddRow(GetLoc("IPNetBIOS"), GetLoc("Enabled"))
                End If
            End If
        Next

    End Sub

    ' *** FÜGGVÉNY: Sor hozzáadása az IP-táblához ***
    ' Bemenet: Name  -> név (String)
    '          Value -> érték (String)
    ' Kimenet: *     -> hamis érték (Boolean)
    Private Function IPTableAddRow(ByVal Name As String, ByVal Value As String)

        ' Értékek definiálása
        Dim ListItem As ListViewItem                            ' Egy sor elemei listanézetben
        Dim ListFields(3) As String                             ' Egy sor értékeinek tömbje (A kezdő oszlop miatt 1-gyel nagyobb!)
        Dim ListColumn As Int32                                 ' Aktuális oszlop száma

        ' Formázás -> Félkövér és normál betűk soron belül
        Dim ListBold() As Boolean = {False, True, False}

        ' Név hozzáadása (1)
        If Name <> Nothing Then
            ListFields(1) = Name + ":"
        End If

        ' Érték hozzáadása (2)
        If Value = "0" Or Value = Nothing Then

            ' Üres érték kezelése
            ListFields(2) = GetLoc("NotAvailable")
        Else

            ' Érték hozzáadása
            ListFields(2) = Value

        End If

        ' Új sor definiálása
        ListItem = New ListViewItem(ListFields)

        ' Elemek önálló formázásának engedélyezése
        ListItem.UseItemStyleForSubItems = False

        ' Fomrázási beállítások
        For ListColumn = 0 To UBound(ListBold)
            If ListBold(ListColumn) Then

                ' Félkövér
                ListItem.SubItems(ListColumn).Font = New Font(IP_Table.Font, FontStyle.Bold)
            Else

                ' Normál
                ListItem.SubItems(ListColumn).Font = New Font(IP_Table.Font, FontStyle.Regular)
            End If

            ' Formátum beállítása
            ListItem.SubItems.Add(ListFields(ListColumn))
        Next

        ' Sor hozzáadása a lsitához

        IP_Table.Items.Add(ListItem)


        ' Visszatérési érték beállítása
        Return False

    End Function

    ' *** FÜGGVÉNY: Netmask átkódolása ***
    ' Bemenet: Raw   -> nyelvi változó neve (String)
    ' Kimenet: Value -> nyelvi változó értéke (String)
    Public Shared Function GetNetmask(ByVal Raw As String)

        ' Értékek definiálása
        Dim MaskIdentifier As Int32 = 0                         ' Mask azonosító
        Dim Value As String = Nothing                           ' Visszatérési érték

        ' Netmask tömb
        Dim NetMaskRaw() As String = {"0.0.0.0",
                                      "128.0.0.0",
                                      "192.0.0.0",
                                      "224.0.0.0",
                                      "240.0.0.0",
                                      "248.0.0.0",
                                      "252.0.0.0",
                                      "254.0.0.0",
                                      "255.0.0.0",
                                      "255.128.0.0",
                                      "255.192.0.0",
                                      "255.224.0.0",
                                      "255.240.0.0",
                                      "255.248.0.0",
                                      "255.252.0.0",
                                      "255.254.0.0",
                                      "255.255.0.0",
                                      "255.255.128.0",
                                      "255.255.192.0",
                                      "255.255.224.0",
                                      "255.255.240.0",
                                      "255.255.248.0",
                                      "255.255.252.0",
                                      "255.255.254.0",
                                      "255.255.255.0",
                                      "255.255.255.128",
                                      "255.255.255.192",
                                      "255.255.255.224",
                                      "255.255.255.240",
                                      "255.255.255.248",
                                      "255.255.255.252",
                                      "255.255.255.254",
                                      "255.255.255.255"}


        ' A tömb végnézése
        For MaskIdentifier = 0 To UBound(NetMaskRaw)

            ' Ha van egyezés, akkor beállítja az értéket
            If Raw = NetMaskRaw(MaskIdentifier) Then
                Value = MaskIdentifier.ToString + " (IPv4)"
            End If
        Next

        ' Ha nem szerepel a tömbben, akkor az eredeti érték visszaadása
        If Value = Nothing Then
            Value = Raw + " (IPv6)"
        End If

        ' Visszatérési érték beállítása
        Return Value

    End Function

    ' *** ELJÁRÁS: Kilépési procedúra megindítása (közvetett) ***
    ' Eseményvezérelt: Me.KeyDown -> ESC (Fizikai gomb lenyomása)
    Private Sub KeyDown_Escape_Close(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        ' Ablak bezárása ESC lenyomására
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If

    End Sub

    ' *** ELJÁRÁS: Kilépési procedúra megindítása (közvetett) ***
    ' Eseményvezérelt: Button_Close.Click -> Klikk (Gomb)
    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click

        ' Ablak bezárása
        Me.Close()

    End Sub

End Class