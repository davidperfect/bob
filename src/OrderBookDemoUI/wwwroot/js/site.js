﻿        function getWebServiceUri(path) {
            return 'http://' + window.location.hostname + ':5000/api/' + path;
        }

        function getWebServiceHub(hubName) {
            return 'http://' + window.location.hostname + ':5000/' + hubName;
        }
