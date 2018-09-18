Namespace CCFramework.Core

    Public Class Settings
        Public Const BreadCrumbDelimiter As String = "/"
        Public Const TestMode As Boolean = True

        Public Const OptimalAccountNumber As String = "1000023167"
        Public Const OptimalStoreId As String = "test"
        Public Const OptimalStorePassword As String = "test"

        Public Const BraintreeMerchantId As String = "9rzxctg858q6zgg4"
        Public Const BraintreePublicKey As String = "3p388vkkxs2b8h9v"
        Public Const BraintreePrivateKey As String = "fc1382d47231cedf6f30a962e5b04e03"

        Public Const AuthorizeDotNetLoginId As String = "2GmGfw3gkL8W"
        Public Const AuthorizeDotNetTransactionKey As String = "57Qj6ra2v626N2Jw"

        Public Const PayPalAccountEmailAddress As String = "daniel@seriousmonkey.ca"

        Public Const MonerisStoreId As String = ""
        Public Const MonerisApiToken As String = ""

        Public Shared Project As String
        Public Shared Version As String
        Public Shared SiteTitle As String
        Public Shared CompanyName As String
        Public Shared Theme As String
        Public Shared GoogleSiteVerification As String
        Public Shared BingSiteVerification As String
        Public Shared Root As String
        Public Shared HostName As String
        Public Shared FileUploadPath As String
        Public Shared LinkFormatString As String
        Public Shared AccessDeniedFile As String
        Public Shared NoImageUrl As String
        Public Shared LogoImageUrl As String

        Public Shared AnalyticsCode As String
        Public Shared GoogleSitemapPath As String

        Public Shared TrackErrors As Boolean
        Public Shared ErrorEmail As String

        Public Shared AdminName As String
        Public Shared AdminEmail As String

        Public Shared SupportEmail As String

        Public Shared SendNoticeEmails As Boolean
        Public Shared SendTrackingEmails As Boolean

        Public Shared EnableSSL As Boolean
        Public Shared FullSSL As Boolean
        Public Shared SSLUrl As String

        Public Shared HomePage As String
        Public Shared SearchPage As String
        Public Shared MenuPage As String
        Public Shared LinkPage As String
        Public Shared EventPage As String
        Public Shared NewsPage As String
        Public Shared TestimonialPage As String
        Public Shared VideosPage As String
        Public Shared GalleryPage As String

        Public Shared PropertySearchPage As String
        Public Shared PropertyResultsPage As String

        Public Shared ProgramsPage As String

        Public Shared ClearancePage As String
        Public Shared TopSellersPage As String
        Public Shared ShopPage As String

        Public Shared StoreEnabled As Boolean
        Public Shared StoreDisabledPage As String
        Public Shared StoreAdminEnabled As Boolean

        Public Shared EnableEmailSSL As Boolean
        Public Shared SMTPPort As Integer
        Public Shared SMTPHost As String
        Public Shared SMTPUser As String
        Public Shared SMTPPass As String

        Public Shared MailFromAddress As String

        Public Shared NotifyDistributors As Boolean
        Public Shared NotifyShippingCompany As Boolean
        Public Shared ShippingCompanyEmail As String

        Public Shared CategoryTitle As String
        Public Shared ProductNamesToDisplay As Integer
        Public Shared DefaultPaymentType As Integer
        Public Shared DefaultCountryID As Integer
        Public Shared NoSizeID As Integer
        Public Shared NoColorID As Integer
        Public Shared CartPrompt As Boolean
        Public Shared ProductUploadPath As String
        Public Shared AllowableFileDowloads As Integer
        Public Shared WebsiteCost As Decimal
        Public Shared StoreCreditCards As Boolean

        Public Shared UseCPRates As Boolean
        Public Shared UseUPSRates As Boolean
        Public Shared UsePurolatorRates As Boolean
        Public Shared UsePickUpRate As Boolean
        Public Shared UseFixedRate As Boolean

        Public Shared CPMerchantID As String

        Public Shared UPSLicenseNumber As String
        Public Shared UPSLicenseUser As String
        Public Shared UPSLicensePassword As String

        Public Shared PurolatorLicenseKey As String
        Public Shared PurolatorLicenseUser As String
        Public Shared PurolatorLicensePassword As String

        Public Shared ShippingMargin As Double
        Public Shared ShowCartWeight As Boolean

        Public Shared UseFreeShipping As Boolean
        Public Shared FreeShippingAfterAmount As Decimal
        Public Shared UseMaxShippingCharge As Boolean
        Public Shared MaxShippingCharge As Decimal
        Public Shared DefaultShippingZone As Integer

        Public Shared ShipFromCity As String
        Public Shared ShipFromProvince As String
        Public Shared ShipFromProvinceCode As String
        Public Shared ShipFromPC As String
        Public Shared ShipFromCountry As String
        Public Shared ShipFromCountryCode As String
        Public Shared ShipFromAddress As String
        Public Shared ShipFromPN As String
        Public Shared ShipFromCompany As String
        Public Shared ShipFromAttention As String


        Public Shared BSMerchantID As String
        Public Shared BSHostAddress As String
        Public Shared BSServiceVersion As String
        Public Shared BSTermURL As String

        Public Shared ReCaptchaPrivateKey As String
        Public Shared ReCaptchaPublicKey As String

        Public Shared FacebookAppID As String
        Public Shared FacebookLikePageID As String
        Public Shared TwitterUser As String
        Public Shared YouTubeAccount As String

        Public Shared BlogDescription As String
        Public Shared RssChannelTitle As String
        Public Shared RssChannelLink As String
        Public Shared RssChannelDescription As String
        Public Shared RssChannelWebMaster As String
        Public Shared RssChannelManagingEditor As String
        Public Shared RssChannelTtl As String
        Public Shared RssChannelLanguage As String
        Public Shared RssChannelCopyright As String
        Public Shared RssChannelCategory As String
        Public Shared RssItemTitle As String
        Public Shared RssItemLink As String
        Public Shared RssItemLogoUrl As String
        Public Shared RssLink As String
        Public Shared RssXmlFile As String

        Public Sub New()
            LoadSettings()
        End Sub

        Public Shared Sub LoadSettings()
            Project = SettingController.GetSettingValue("Project")
            Version = "1.058" 'SettingController.GetSettingValue("Version")
            SiteTitle = SettingController.GetSettingValue("SiteTitle")
            CompanyName = SettingController.GetSettingValue("CompanyName")
            Theme = "Default" 'SettingController.GetSettingValue("Theme")
            GoogleSiteVerification = "BB1iHxjwiibg5PkKG2_X6SWqZXeYREco-HZfC_jYYw0" 'SettingController.GetSettingValue("GoogleSiteVerification")
            BingSiteVerification = "D74B86D62D44AEA3E9649A5114E5C1BF" 'SettingController.GetSettingValue("BingSiteVerification")
            Root = SettingController.GetSettingValue("Root")
            HostName = SettingController.GetSettingValue("HostName")
            FileUploadPath = SettingController.GetSettingValue("FileUploadPath")
            LinkFormatString = SettingController.GetSettingValue("LinkFormatString")
            AccessDeniedFile = SettingController.GetSettingValue("AccessDeniedFile")
            NoImageUrl = SettingController.GetSettingValue("NoImageUrl")
            LogoImageUrl = SettingController.GetSettingValue("LogoImageUrl")

            AnalyticsCode = SettingController.GetSettingValue("AnalyticsCode")
            GoogleSitemapPath = SettingController.GetSettingValue("GoogleSitemapPath")

            TrackErrors = Boolean.Parse(SettingController.GetSettingValue("TrackErrors"))
            ErrorEmail = SettingController.GetSettingValue("ErrorEmail")

            AdminName = SettingController.GetSettingValue("AdminName")
            AdminEmail = SettingController.GetSettingValue("AdminEmail")

            SupportEmail = SettingController.GetSettingValue("SupportEmail")

            EnableSSL = Boolean.Parse(SettingController.GetSettingValue("EnableSSL"))
            FullSSL = Boolean.Parse(SettingController.GetSettingValue("FullSSL"))
            SSLUrl = SettingController.GetSettingValue("SSLUrl")

            HomePage = SettingController.GetSettingValue("HomePagePermalink")
            SearchPage = SettingController.GetSettingValue("SearchPagePermalink")
            MenuPage = SettingController.GetSettingValue("MenuPagePermalink")
            LinkPage = SettingController.GetSettingValue("LinkPagePermalink")
            EventPage = SettingController.GetSettingValue("EventPagePermalink")
            NewsPage = SettingController.GetSettingValue("NewsPagePermalink")
            TestimonialPage = SettingController.GetSettingValue("TestimonialPagePermalink")
            VideosPage = SettingController.GetSettingValue("VideoPagePermalink")
            GalleryPage = SettingController.GetSettingValue("GalleryPagePermalink")

            PropertySearchPage = SettingController.GetSettingValue("PropertySearchPagePermalink")
            PropertyResultsPage = SettingController.GetSettingValue("PropertyResultsPagePermalink")

            ProgramsPage = SettingController.GetSettingValue("ProgramsPagePermalink")

            ClearancePage = SettingController.GetSettingValue("ClearancePagePermalink")
            TopSellersPage = SettingController.GetSettingValue("TopSellersPagePermalink")
            ShopPage = SettingController.GetSettingValue("ShopPagePermalink")

            StoreEnabled = Boolean.Parse(SettingController.GetSettingValue("StoreEnabled"))
            StoreDisabledPage = SettingController.GetSettingValue("StoreDisabledPage")
            StoreAdminEnabled = Boolean.Parse(SettingController.GetSettingValue("StoreAdminEnabled"))

            SendNoticeEmails = Boolean.Parse(SettingController.GetSettingValue("SendNoticeEmails"))
            SendTrackingEmails = Boolean.Parse(SettingController.GetSettingValue("SendTrackingEmails"))

            EnableEmailSSL = Boolean.Parse(SettingController.GetSettingValue("EnableEmailSSL"))
            SMTPPort = Integer.Parse(SettingController.GetSettingValue("SMTPPort"))
            SMTPHost = SettingController.GetSettingValue("SMTPHost")
            SMTPUser = SettingController.GetSettingValue("SMTPUser")
            SMTPPass = SettingController.GetSettingValue("SMTPPass")

            MailFromAddress = SettingController.GetSettingValue("MailFromAddress")

            NotifyDistributors = Boolean.Parse(SettingController.GetSettingValue("NotifyDistributors"))
            NotifyShippingCompany = Boolean.Parse(SettingController.GetSettingValue("NotifyShippingCompany"))
            ShippingCompanyEmail = SettingController.GetSettingValue("ShippingCompanyEmail")

            CategoryTitle = SettingController.GetSettingValue("CategoryTitle")
            ProductNamesToDisplay = Integer.Parse(SettingController.GetSettingValue("ProductNamesToDisplay"))
            DefaultPaymentType = Integer.Parse(SettingController.GetSettingValue("DefaultPaymentType"))
            DefaultCountryID = Integer.Parse(SettingController.GetSettingValue("DefaultCountryID"))
            NoSizeID = Integer.Parse(SettingController.GetSettingValue("NoSizeID"))
            NoColorID = Integer.Parse(SettingController.GetSettingValue("NoColorID"))
            CartPrompt = Boolean.Parse(SettingController.GetSettingValue("CartPrompt"))
            ProductUploadPath = SettingController.GetSettingValue("ProductUploadPath")
            AllowableFileDowloads = Integer.Parse(SettingController.GetSettingValue("AllowableFileDownloads"))
            WebsiteCost = Decimal.Parse(SettingController.GetSettingValue("WebsiteCost"))
            StoreCreditCards = Boolean.Parse(SettingController.GetSettingValue("StoreCreditCards"))

            UseCPRates = Boolean.Parse(SettingController.GetSettingValue("UseCPRates"))
            UseUPSRates = Boolean.Parse(SettingController.GetSettingValue("UseUPSRates"))
            UsePurolatorRates = Boolean.Parse(SettingController.GetSettingValue("UsePurolatorRates"))
            UsePickUpRate = Boolean.Parse(SettingController.GetSettingValue("UsePickupRate"))
            UseFixedRate = Boolean.Parse(SettingController.GetSettingValue("UseFixedRate"))

            CPMerchantID = SettingController.GetSettingValue("CPMerchantID")

            UPSLicenseNumber = SettingController.GetSettingValue("UPSLicenseNumber")
            UPSLicenseUser = SettingController.GetSettingValue("UPSLicenseUser")
            UPSLicensePassword = SettingController.GetSettingValue("UPSLicensePassword")

            PurolatorLicenseKey = SettingController.GetSettingValue("PurolatorLicenseKey")
            PurolatorLicenseUser = SettingController.GetSettingValue("PurolatorUserId")
            PurolatorLicensePassword = SettingController.GetSettingValue("PurolatorPassword")

            ShippingMargin = Double.Parse(SettingController.GetSettingValue("ShippingMargin"))
            ShowCartWeight = Boolean.Parse(SettingController.GetSettingValue("ShowCartWeight"))

            UseFreeShipping = Boolean.Parse(SettingController.GetSettingValue("UseFreeShipping"))
            FreeShippingAfterAmount = Decimal.Parse(SettingController.GetSettingValue("FreeShippingAfterAmount"))
            UseMaxShippingCharge = Boolean.Parse(SettingController.GetSettingValue("UseMaxShippingCharge"))
            MaxShippingCharge = Decimal.Parse(SettingController.GetSettingValue("MaxShippingCharge"))
            DefaultShippingZone = Integer.Parse(SettingController.GetSettingValue("DefaultShippingZone"))

            ShipFromCity = SettingController.GetSettingValue("ShipFromCity")
            ShipFromProvince = SettingController.GetSettingValue("ShipFromProvince")
            ShipFromProvinceCode = SettingController.GetSettingValue("ShipFromProvinceCode")
            ShipFromPC = SettingController.GetSettingValue("ShipFromPC")
            ShipFromCountry = SettingController.GetSettingValue("ShipFromCountry")
            ShipFromCountryCode = SettingController.GetSettingValue("ShipFromCountryCode")
            ShipFromAddress = SettingController.GetSettingValue("ShipFromAddress")
            ShipFromPN = SettingController.GetSettingValue("ShipFromPN")
            ShipFromCompany = SettingController.GetSettingValue("ShipFromCompany")
            ShipFromAttention = SettingController.GetSettingValue("ShipFromAttention")

            BSMerchantID = SettingController.GetSettingValue("BSMerchantID")
            BSServiceVersion = SettingController.GetSettingValue("BSServiceVersion")
            BSHostAddress = SettingController.GetSettingValue("BSHostAddress")
            BSTermURL = SettingController.GetSettingValue("BSTermURL")

            ReCaptchaPrivateKey = SettingController.GetSettingValue("ReCaptchaPrivateKey")
            ReCaptchaPublicKey = SettingController.GetSettingValue("ReCaptchaPublicKey")

            FacebookAppID = SettingController.GetSettingValue("FBAppID")
            FacebookLikePageID = SettingController.GetSettingValue("FBLikePageID")
            TwitterUser = SettingController.GetSettingValue("TwitterUser")
            YouTubeAccount = SettingController.GetSettingValue("YouTubeAccount")

            BlogDescription = SettingController.GetSettingValue("BlogDescription")
            RssChannelTitle = SettingController.GetSettingValue("RssChannelTitle")
            RssChannelLink = SettingController.GetSettingValue("RssChannelLink")
            RssChannelDescription = SettingController.GetSettingValue("RssChannelDescription")
            RssChannelWebMaster = SettingController.GetSettingValue("RssChannelWebMaster")
            RssChannelManagingEditor = SettingController.GetSettingValue("RssChannelManagingEditor")
            RssChannelTtl = SettingController.GetSettingValue("RssChannelTtl")
            RssChannelLanguage = SettingController.GetSettingValue("RssChannelLanguage")
            RssChannelCopyright = SettingController.GetSettingValue("RssChannelCopyright")
            RssChannelCategory = SettingController.GetSettingValue("RssChannelCategory")
            RssItemTitle = SettingController.GetSettingValue("RssItemTitle")
            RssItemLink = SettingController.GetSettingValue("RssItemLink")
            RssItemLogoUrl = SettingController.GetSettingValue("RssItemLogoUrl")
            RssLink = SettingController.GetSettingValue("RssLink")
            RssXmlFile = SettingController.GetSettingValue("RssXmlFile")

        End Sub

        Public Shared Function PrintValues() As String
            Dim Output As New StringBuilder

            Output.Append("Project - " & Project & "<br />")
            Output.Append("SiteTitle - " & SiteTitle & "<br />")
            Output.Append("CompanyName - " & CompanyName & "<br />")
            Output.Append("Theme - " & Theme & "<br />")
            Output.Append("Root - " & Root & "<br />")
            Output.Append("HostName - " & HostName & "<br />")
            Output.Append("FileUploadPath - " & FileUploadPath & "<br />")
            Output.Append("LinkFormatString - " & LinkFormatString & "<br />")
            Output.Append("AccessDeniedFile - " & AccessDeniedFile & "<br />")
            Output.Append("NoImageUrl - " & NoImageUrl & "<br />")
            Output.Append("LogoImageUrl - " & LogoImageUrl & "<br /><br />")

            Output.Append("AnalyticsCode - " & AnalyticsCode & "<br />")
            Output.Append("GoogleSitemapPath - " & GoogleSitemapPath & "<br /><br />")

            Output.Append("TrackErrors - " & TrackErrors.ToString & "<br />")
            Output.Append("ErrorEmail - " & ErrorEmail & "<br /><br />")

            Output.Append("AdminName - " & AdminName & "<br />")
            Output.Append("AdminEmail - " & AdminEmail & "<br /><br />")

            Output.Append("SupportEmail - " & SupportEmail & "<br /><br />")

            Output.Append("EnableSSL - " & EnableSSL.ToString & "<br />")
            Output.Append("FullSSL - " & FullSSL.ToString & "<br />")
            Output.Append("SSLUrl - " & SSLUrl & "<br /><br />")

            Output.Append("HomePage - " & HomePage & "<br />")
            Output.Append("SearchPage - " & SearchPage & "<br />")
            Output.Append("MenuPage - " & MenuPage & "<br />")
            Output.Append("LinkPage - " & LinkPage & "<br />")
            Output.Append("EventPage - " & EventPage & "<br />")
            Output.Append("NewsPage - " & NewsPage & "<br />")
            Output.Append("TestimonialPage - " & TestimonialPage & "<br />")
            Output.Append("VideosPage - " & VideosPage & "<br />")
            Output.Append("GalleryPage - " & GalleryPage & "<br /><br />")

            Output.Append("PropertySearchPage - " & PropertySearchPage & "<br />")
            Output.Append("PropertyResultsPage - " & PropertyResultsPage & "<br /><br />")

            Output.Append("ProgramsPage - " & ProgramsPage & "<br /><br />")

            Output.Append("ClearancePage - " & ClearancePage & "<br />")
            Output.Append("TopSellersPage - " & TopSellersPage & "<br />")
            Output.Append("ShopPage - " & ShopPage & "<br /><br />")

            Output.Append("StoreEnabled - " & StoreEnabled.ToString & "<br />")
            Output.Append("StoreDisabledPage - " & StoreDisabledPage & "<br />")
            Output.Append("StoreAdminEnabled - " & StoreAdminEnabled.ToString & "<br /><br />")

            Output.Append("SendNoticeEmails - " & SendNoticeEmails.ToString & "<br />")
            Output.Append("SendTrackingEmails - " & SendTrackingEmails.ToString & "<br /><br />")

            Output.Append("EnableEmailSSL - " & EnableEmailSSL.ToString & "<br />")
            Output.Append("SMTPPort - " & SMTPPort.ToString & "<br />")
            Output.Append("SMTPHost - " & SMTPHost & "<br />")
            Output.Append("SMTPUser - " & SMTPUser & "<br />")
            Output.Append("SMTPPass - " & SMTPPass & "<br /><br />")

            Output.Append("MailFromAddress - " & MailFromAddress & "<br /><br />")

            Output.Append("NotifyDistributors - " & NotifyDistributors.ToString & "<br />")
            Output.Append("NotifyShippingCompany - " & NotifyShippingCompany.ToString & "<br />")
            Output.Append("ShippingCompanyEmail - " & ShippingCompanyEmail & "<br /><br />")

            Output.Append("CategoryTitle - " & CategoryTitle & "<br />")
            Output.Append("ProductNamesToDisplay - " & ProductNamesToDisplay.ToString & "<br />")
            Output.Append("DefaultPaymentType - " & DefaultPaymentType.ToString & "<br />")
            Output.Append("DefaultCountryID - " & DefaultCountryID.ToString & "<br />")
            Output.Append("NoSizeID - " & NoSizeID.ToString & "<br />")
            Output.Append("NoColorID - " & NoColorID.ToString & "<br />")
            Output.Append("CartPrompt - " & CartPrompt.ToString & "<br />")
            Output.Append("ProductUploadPath - " & ProductUploadPath & "<br />")
            Output.Append("AllowableFileDowloads - " & AllowableFileDowloads.ToString & "<br />")
            Output.Append("WebsiteCost - " & WebsiteCost.ToString & "<br />")
            Output.Append("StoreCreditCards - " & StoreCreditCards.ToString & "<br /><br />")

            Output.Append("UseCPRates - " & UseCPRates.ToString & "<br />")
            Output.Append("UseUPSRates - " & UseUPSRates.ToString & "<br />")
            Output.Append("UsePurolatorRates - " & UsePurolatorRates.ToString & "<br />")
            Output.Append("UsePickUpRate - " & UsePickUpRate.ToString & "<br />")
            Output.Append("UseFixedRate - " & UseFixedRate.ToString & "<br /><br />")

            Output.Append("CPMerchantID - " & CPMerchantID & "<br /><br />")

            Output.Append("UPSLicenseNumber - " & UPSLicenseNumber & "<br />")
            Output.Append("UPSLicenseUser - " & UPSLicenseUser & "<br />")
            Output.Append("UPSLicensePassword - " & UPSLicensePassword & "<br /><br />")

            Output.Append("PurolatorLicenseKey - " & PurolatorLicenseKey & "<br />")
            Output.Append("PurolatorLicenseUser - " & PurolatorLicenseUser & "<br />")
            Output.Append("PurolatorLicensePassword - " & PurolatorLicensePassword & "<br /><br />")

            Output.Append("ShippingMargin - " & ShippingMargin.ToString & "<br />")
            Output.Append("ShowCartWeight - " & ShowCartWeight.ToString & "<br /><br />")

            Output.Append("UseFreeShipping - " & UseFreeShipping.ToString & "<br />")
            Output.Append("FreeShippingAfterAmount - " & FreeShippingAfterAmount.ToString & "<br />")
            Output.Append("UseMaxShippingCharge - " & UseMaxShippingCharge.ToString & "<br />")
            Output.Append("MaxShippingCharge - " & MaxShippingCharge.ToString & "<br />")
            Output.Append("DefaultShippingZone - " & DefaultShippingZone.ToString & "<br /><br />")

            Output.Append("ShipFromCity - " & ShipFromCity & "<br />")
            Output.Append("ShipFromProvince - " & ShipFromProvince & "<br />")
            Output.Append("ShipFromProvinceCode - " & ShipFromProvinceCode & "<br />")
            Output.Append("ShipFromPC - " & ShipFromPC & "<br />")
            Output.Append("ShipFromCountry - " & ShipFromCountry & "<br />")
            Output.Append("ShipFromCountryCode - " & ShipFromCountryCode & "<br />")
            Output.Append("ShipFromAddress - " & ShipFromAddress & "<br />")
            Output.Append("ShipFromPN - " & ShipFromPN & "<br />")
            Output.Append("ShipFromCompany - " & ShipFromCompany & "<br />")
            Output.Append("ShipFromAttention - " & ShipFromAttention & "<br /><br />")

            Output.Append("BSMerchantID - " & BSMerchantID & "<br />")
            Output.Append("BSServiceVersion - " & BSServiceVersion & "<br />")
            Output.Append("BSHostAddress - " & BSHostAddress & "<br />")
            Output.Append("BSTermURL - " & BSTermURL & "<br /><br />")

            Output.Append("ReCaptchaPrivateKey - " & ReCaptchaPrivateKey & "<br />")
            Output.Append("ReCaptchaPublicKey - " & ReCaptchaPublicKey & "<br /><br />")

            Output.Append("FacebookAppID - " & FacebookAppID & "<br />")
            Output.Append("FacebookLikePageID - " & FacebookLikePageID & "<br />")
            Output.Append("TwitterUser - " & TwitterUser & "<br />")
            Output.Append("YouTubeAccount - " & YouTubeAccount & "<br /><br />")

            Output.Append("BlogDescription - " & BlogDescription & "<br />")
            Output.Append("RssChannelTitle - " & RssChannelTitle & "<br />")
            Output.Append("RssChannelLink - " & RssChannelLink & "<br />")
            Output.Append("RssChannelDescription - " & RssChannelDescription & "<br />")
            Output.Append("RssChannelWebMaster - " & RssChannelWebMaster & "<br />")
            Output.Append("RssChannelManagingEditor - " & RssChannelManagingEditor & "<br />")
            Output.Append("RssChannelTtl - " & RssChannelTtl & "<br />")
            Output.Append("RssChannelLanguage - " & RssChannelLanguage & "<br />")
            Output.Append("RssChannelCopyright - " & RssChannelCopyright & "<br />")
            Output.Append("RssChannelCategory - " & RssChannelCategory & "<br />")
            Output.Append("RssItemTitle - " & RssItemTitle & "<br />")
            Output.Append("RssItemLink - " & RssItemLink & "<br />")
            Output.Append("RssItemLogoUrl - " & RssItemLogoUrl & "<br />")
            Output.Append("RssLink - " & RssLink & "<br />")
            Output.Append("RssXmlFile - " & RssXmlFile & "<br /><br />")

            Return Output.ToString
        End Function

    End Class

End Namespace