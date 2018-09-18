Namespace CCFramework.Commerce.Shipping.UPS

    Public Enum ServiceType
        All = -1
        UPSNextDayAir = 1
        UPSSecondDayAir = 2
        UPSGround = 3
        UPSWorldwideExpress = 7
        UPSWorldwideExpedited = 8
        UPSStandard = 11
        UPSThreeDaySelect = 12
        UPSNextDayAirSaver = 13
        UPSNextDayAirEarlyAM = 14
        UPSWorldwideExpressPlus = 54
        UPSSecondDayAirAM = 59
        UPSSaver = 65
        UPSTodayStandard = 82
        UPSTodayDedicatedCourrier = 83
        UPSTodayIntercity = 84
        UPSTodayExpress = 85
        UPSTodayExpressSaver = 86
        TradeDirectCrossBorder = 305
        TradeDirectAir = 306
        TradeDirectOcean = 307
        UPSFreightLTL = 308
        UPSFreightLTLGuaranteed = 309
        UPSFreightLTLUrgent = 310

        'Code United States Domestic Shipments
        '01 UPS Next Day Air
        '02 UPS Second Day Air
        '03 UPS Ground
        '12 UPS Three-Day Select
        '13 UPS Next Day Air Saver
        '14 UPS Next Day Air® Early A.M. SM
        '59 UPS Second Day Air A.M.
        '65 UPS Saver
        'Code Shipments Originating in United States
        '01 UPS Next Day Air
        '02 UPS Second Day Air
        '03 UPS Ground
        '07 UPS Worldwide Express
        '08 UPS Worldwide Expedited
        '11 UPS Standard
        '12 UPS Three-Day Select
        '14 UPS Next Day Air® Early A.M. 
        '54 UPS Worldwide Express Plus
        '59 UPS Second Day Air A.M.
        '65 UPS Saver
        'Code Shipments Originating in Puerto Rico
        '01 UPS Next Day Air
        '02 UPS Second Day Air
        '03 UPS Ground
        '07 UPS Worldwide Express
        '08 UPS Worldwide Expedited
        '14 UPS Next Day Air® Early A.M.
        '54 UPS Worldwide Express Plus
        '65 UPS Saver
        'Code Shipments Originating in Canada
        '01 UPS Express
        '02 UPS Expedited
        '07 UPS Worldwide Express
        '08 UPS Worldwide Expedited
        '11 UPS Standard
        '12 UPS Three-Day Select
        '13 UPS Saver
        '14 UPS Express Early A.M.
        '07 UPS Express
        '08 UPS Expedited
        '54 UPS Express Plus
        '65 UPS Saver
        'Code Polish Domestic Shipments
        '07 UPS Express
        '08 UPS Expedited
        '11 UPS Standard
        '54 UPS Worldwide Express Plus
        '65 UPS Saver
        '82 UPS Today Standard
        '83 UPS Today Dedicated Courrier
        '84 UPS Today Intercity
        '85 UPS Today Express
        '86 UPS Today Express Saver
        'Code Shipments Originating in the European Union
        '07 UPS Express
        '08 UPS Expedited
        '11 UPS Standard
        '54 UPS Worldwide Express Plus
        '65 UPS Saver
        'Code Shipments Originating in Other Countries
        '07 UPS Express
        '08 UPS Worldwide Expedited
        '11 UPS Standard
        '54 UPS Worldwide Express Plus
        '65 UPS Saver
        'Code Freight Shipments
        'TDCB Trade Direct Cross Border
        'TDA Trade Direct Air
        'TDO Trade Direct Ocean
        '308 UPS Freight LTL
        '309 UPS Freight LTL Guaranteed
        '310 UPS Freight LTL Urgent
    End Enum

End Namespace