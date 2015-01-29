(function (constants, undefined) {

    constants.MAP_STYLES = [
        {
            "featureType": "landscape",
            "stylers": [
                { "visibility": "on" },
                { "color": "#ffffff" }
            ]
        }, {
            "featureType": "road",
            "elementType": "geometry",
            "stylers": [
                { "visibility": "on" },
                { "color": "#d9d9d9" }
            ]
        }, {
            "featureType": "poi.attraction",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.business",
            "stylers": [
                { "visibility": "on" },
                { "color": "#d9d9d9" }
            ]
        }, {
            "featureType": "poi.government",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.medical",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.park",
            "stylers": [
                { "visibility": "on" },
                { "color": "#53a93f" },
                { "lightness": 37 }
            ]
        }, {
            "featureType": "poi.place_of_worship",
            "stylers": [
                { "visibility": "off" }
            ]
        }, {
            "featureType": "poi.school",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.sports_complex",
            "stylers": [
                { "visibility": "off" }
            ]
        }, {
            "featureType": "water",
            "stylers": [
                { "visibility": "on" },
                { "color": "#049cdb" }
            ]
        }, {
            "featureType": "poi.business",
            "elementType": "labels"
        }, {

        }, {
            "featureType": "administrative",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "elementType": "labels.text.fill",
            "stylers": [
                { "visibility": "on" },
                { "color": "#1d1d1d" }
            ]
        }, {
            "elementType": "labels.text.stroke",
            "stylers": [
                { "visibility": "on" },
                { "color": "#ffffff" }
            ]
        }, {

        }
    ];

    constants.MARKER_ICON = {
        path: 'M0-165c-27.618 0-50 21.966-50 49.054C-50-88.849 0 0 0 0s50-88.849 50-115.946C50-143.034 27.605-165 0-165z',
        fillColor: '#ff7100',
        fillOpacity: 1,
        strokeColor: '#000000',
        strokeWeight: 2,
        scale: 1 / 4
    };

    constants.COUNTRIES = [
        {
            "countryCode": "AF",
            "name": "Afghanistan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "AL",
            "name": "Albania",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "DZ",
            "name": "Algeria",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "AR",
            "name": "Argentina",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "AM",
            "name": "Armenia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "AU",
            "name": "Australia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "AT",
            "name": "Austria",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "AZ",
            "name": "Azerbaijan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BH",
            "name": "Bahrain",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BD",
            "name": "Bangladesh",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BY",
            "name": "Belarus",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BE",
            "name": "Belgium",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BZ",
            "name": "Belize",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "VE",
            "name": "Bolivarian Republic of Venezuela",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BO",
            "name": "Bolivia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BA",
            "name": "Bosnia and Herzegovina",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BW",
            "name": "Botswana",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BR",
            "name": "Brazil",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BN",
            "name": "Brunei Darussalam",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "BG",
            "name": "Bulgaria",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "KH",
            "name": "Cambodia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CA",
            "name": "Canada",
            "provinceLabel": "Province",
            "provinces": [
                {
                    "name": "Alberta",
                    "code": "AB"
                },
                {
                    "name": "British Columbia",
                    "code": "BC"
                },
                {
                    "name": "Manitoba",
                    "code": "MB"
                },
                {
                    "name": "New Brunswick",
                    "code": "NB"
                },
                {
                    "name": "Newfoundland and Labrador",
                    "code": "NL"
                },
                {
                    "name": "Northwest Territories",
                    "code": "NT"
                },
                {
                    "name": "Nova Scotia",
                    "code": "NS"
                },
                {
                    "name": "Nunavut",
                    "code": "NU"
                },
                {
                    "name": "Ontario",
                    "code": "ON"
                },
                {
                    "name": "Prince Edward Island",
                    "code": "PE"
                },
                {
                    "name": "Quebec",
                    "code": "QC"
                },
                {
                    "name": "Saskatchewan",
                    "code": "SK"
                },
                {
                    "name": "Yukon",
                    "code": "YT"
                }
            ]
        },
        {
            "countryCode": "029",
            "name": "Caribbean",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CL",
            "name": "Chile",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CN",
            "name": "China",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CO",
            "name": "Colombia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CR",
            "name": "Costa Rica",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "HR",
            "name": "Croatia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CZ",
            "name": "Czech Republic",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "DK",
            "name": "Denmark",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "DO",
            "name": "Dominican Republic",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "EC",
            "name": "Ecuador",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "EG",
            "name": "Egypt",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SV",
            "name": "El Salvador",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "ER",
            "name": "Eritrea",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "EE",
            "name": "Estonia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "ET",
            "name": "Ethiopia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "FO",
            "name": "Faroe Islands",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "FI",
            "name": "Finland",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "FR",
            "name": "France",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "GE",
            "name": "Georgia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "DE",
            "name": "Germany",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "GR",
            "name": "Greece",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "GL",
            "name": "Greenland",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "GT",
            "name": "Guatemala",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "HN",
            "name": "Honduras",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "HK",
            "name": "Hong Kong SAR",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "HU",
            "name": "Hungary",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "IS",
            "name": "Iceland",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "IN",
            "name": "India",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "ID",
            "name": "Indonesia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "IR",
            "name": "Iran",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "IQ",
            "name": "Iraq",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "IE",
            "name": "Ireland",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "IL",
            "name": "Israel",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "IT",
            "name": "Italy",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "JM",
            "name": "Jamaica",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "JP",
            "name": "Japan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "JO",
            "name": "Jordan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "KZ",
            "name": "Kazakhstan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "KE",
            "name": "Kenya",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "KR",
            "name": "Korea",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "KW",
            "name": "Kuwait",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "KG",
            "name": "Kyrgyzstan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LA",
            "name": "Lao PDR",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LV",
            "name": "Latvia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LB",
            "name": "Lebanon",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LY",
            "name": "Libya",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LI",
            "name": "Liechtenstein",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LT",
            "name": "Lithuania",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LU",
            "name": "Luxembourg",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MO",
            "name": "Macao SAR",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MK",
            "name": "Macedonia (Former Yugoslav Republic of Macedonia)",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MY",
            "name": "Malaysia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MV",
            "name": "Maldives",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MT",
            "name": "Malta",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MX",
            "name": "Mexico",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MN",
            "name": "Mongolia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "ME",
            "name": "Montenegro",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MA",
            "name": "Morocco",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "NP",
            "name": "Nepal",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "NL",
            "name": "Netherlands",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "NZ",
            "name": "New Zealand",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "NI",
            "name": "Nicaragua",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "NG",
            "name": "Nigeria",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "NO",
            "name": "Norway",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "OM",
            "name": "Oman",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PK",
            "name": "Pakistan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PA",
            "name": "Panama",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PY",
            "name": "Paraguay",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PE",
            "name": "Peru",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PH",
            "name": "Philippines",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PL",
            "name": "Poland",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PT",
            "name": "Portugal",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "MC",
            "name": "Principality of Monaco",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "PR",
            "name": "Puerto Rico",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "QA",
            "name": "Qatar",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "RO",
            "name": "Romania",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "RU",
            "name": "Russia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "RW",
            "name": "Rwanda",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SA",
            "name": "Saudi Arabia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SN",
            "name": "Senegal",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "RS",
            "name": "Serbia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CS",
            "name": "Serbia and Montenegro (Former)",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SG",
            "name": "Singapore",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SK",
            "name": "Slovakia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SI",
            "name": "Slovenia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "ZA",
            "name": "South Africa",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "ES",
            "name": "Spain",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "LK",
            "name": "Sri Lanka",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SE",
            "name": "Sweden",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "CH",
            "name": "Switzerland",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "SY",
            "name": "Syria",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "TW",
            "name": "Taiwan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "TJ",
            "name": "Tajikistan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "TH",
            "name": "Thailand",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "TT",
            "name": "Trinidad and Tobago",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "TN",
            "name": "Tunisia",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "TR",
            "name": "Turkey",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "TM",
            "name": "Turkmenistan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "AE",
            "name": "U.A.E.",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "UA",
            "name": "Ukraine",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "GB",
            "name": "United Kingdom",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "US",
            "name": "United States",
            "provinceLabel": "State",
            "provinces": [
                {
                    "name": "Alabama",
                    "code": "AL"
                },
                {
                    "name": "Alaska",
                    "code": "AK"
                },
                {
                    "name": "Arizona",
                    "code": "AZ"
                },
                {
                    "name": "Arkansas",
                    "code": "AR"
                },
                {
                    "name": "California",
                    "code": "CA"
                },
                {
                    "name": "Colorado",
                    "code": "CO"
                },
                {
                    "name": "Connecticut",
                    "code": "CT"
                },
                {
                    "name": "Delaware",
                    "code": "DE"
                },
                {
                    "name": "Florida",
                    "code": "FL"
                },
                {
                    "name": "Georgia",
                    "code": "GA"
                },
                {
                    "name": "Hawaii",
                    "code": "HI"
                },
                {
                    "name": "Idaho",
                    "code": "ID"
                },
                {
                    "name": "Illinois",
                    "code": "IL"
                },
                {
                    "name": "Indiana",
                    "code": "IN"
                },
                {
                    "name": "Iowa",
                    "code": "IA"
                },
                {
                    "name": "Kansas",
                    "code": "KS"
                },
                {
                    "name": "Kentucky",
                    "code": "KY"
                },
                {
                    "name": "Louisiana",
                    "code": "LA"
                },
                {
                    "name": "Maine",
                    "code": "ME"
                },
                {
                    "name": "Maryland",
                    "code": "MD"
                },
                {
                    "name": "Massachusetts",
                    "code": "MA"
                },
                {
                    "name": "Michigan",
                    "code": "MI"
                },
                {
                    "name": "Minnesota",
                    "code": "MN"
                },
                {
                    "name": "Mississippi",
                    "code": "MS"
                },
                {
                    "name": "Missouri",
                    "code": "MO"
                },
                {
                    "name": "Montana",
                    "code": "MT"
                },
                {
                    "name": "Nebraska",
                    "code": "NE"
                },
                {
                    "name": "Nevada",
                    "code": "NV"
                },
                {
                    "name": "New Hampshire",
                    "code": "NH"
                },
                {
                    "name": "New Jersey",
                    "code": "NJ"
                },
                {
                    "name": "New Mexico",
                    "code": "NM"
                },
                {
                    "name": "New York",
                    "code": "NY"
                },
                {
                    "name": "North Carolina",
                    "code": "NC"
                },
                {
                    "name": "North Dakota",
                    "code": "ND"
                },
                {
                    "name": "Ohio",
                    "code": "OH"
                },
                {
                    "name": "Oklahoma",
                    "code": "OK"
                },
                {
                    "name": "Oregon",
                    "code": "OR"
                },
                {
                    "name": "Pennsylvania",
                    "code": "PA"
                },
                {
                    "name": "Rhode Island",
                    "code": "RI"
                },
                {
                    "name": "South Carolina",
                    "code": "SC"
                },
                {
                    "name": "South Dakota",
                    "code": "SD"
                },
                {
                    "name": "Tennessee",
                    "code": "TN"
                },
                {
                    "name": "Texas",
                    "code": "TX"
                },
                {
                    "name": "Utah",
                    "code": "UT"
                },
                {
                    "name": "Vermont",
                    "code": "VT"
                },
                {
                    "name": "Virginia",
                    "code": "VA"
                },
                {
                    "name": "Washington",
                    "code": "WA"
                },
                {
                    "name": "West Virginia",
                    "code": "WV"
                },
                {
                    "name": "Wisconsin",
                    "code": "WI"
                },
                {
                    "name": "Wyoming",
                    "code": "WY"
                },
                {
                    "name": "American Samoa",
                    "code": "AS"
                },
                {
                    "name": "District of Columbia",
                    "code": "DC"
                },
                {
                    "name": "Federated States of Micronesia",
                    "code": "FM"
                },
                {
                    "name": "Guam",
                    "code": "GU"
                },
                {
                    "name": "Marshall Islands",
                    "code": "MH"
                },
                {
                    "name": "Northern Mariana Islands",
                    "code": "MP"
                },
                {
                    "name": "Palau",
                    "code": "PW"
                },
                {
                    "name": "Puerto Rico",
                    "code": "PR"
                },
                {
                    "name": "Virgin Islands",
                    "code": "VI"
                },
                {
                    "name": "Armed Forces - Africa, Canada, Europe",
                    "code": "AE"
                },
                {
                    "name": "Armed Forces Americas",
                    "code": "AA"
                },
                {
                    "name": "Armed Forces Pacific",
                    "code": "AP"
                }
            ]
        },
        {
            "countryCode": "UY",
            "name": "Uruguay",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "UZ",
            "name": "Uzbekistan",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "VN",
            "name": "Vietnam",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "YE",
            "name": "Yemen",
            "provinceLabel": "",
            "provinces": []
        },
        {
            "countryCode": "ZW",
            "name": "Zimbabwe",
            "provinceLabel": "",
            "provinces": []
        }
    ];

    constants.LOCATION_TYPES = [];

    constants.DEFAULT_LOCATION_TYPE_KEY = "30304545-4331-4639-2d37-3135322d3442";

}(window.uLocate.Constants = window.uLocate.Constants || {}));