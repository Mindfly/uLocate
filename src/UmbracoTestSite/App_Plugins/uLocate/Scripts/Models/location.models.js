(function(models, undefined) {

    models.GetLocationsApiRequest = function(data) {
        var self = this;
        if (data === undefined) {
            self.page = 0;
            self.perPage = 100;
            self.sortBy = '';
            self.sortOrder = '';
            self.filter = '';
        } else {
            self.page = data.page;
            self.perPage = data.perPage;
            self.sortBy = data.sortBy;
            self.sortOrder = data.sortOrder;
            self.filter = data.filter;
        }
    };

    models.Location = function(data) {
        var self = this;
        if (data === undefined) {
            self.address1 = '';
            self.address2 = '';
            self.countryCode = '';
            self.customPropertyData = [];
            self.email = '';
            self.key = '00000000-0000-0000-0000-000000000000';
            self.latitude = 0;
            self.locality = '';
            self.locationTypeKey = '00000000-0000-0000-0000-000000000000';
            self.locationTypeName = '';
            self.longitude = 0;
            self.name = '';
            self.phone = '';
            self.postalCode = '';
            self.region = '';
        } else {
            self.address1 = data.address1;
            self.address2 = data.address2;
            self.countryCode = data.countryCode;
            self.customPropertyData = _.map(data.customPropertyData, function(property) {
                return new uLocate.Models.LocationProperty(property);
            });
            if ((typeof data.email)!== "object") {
                self.email = data.email;
            } else {
                self.email = '';
            }
            self.key = data.key;
            self.latitude = data.latitude;
            self.locality = data.locality;
            self.locationTypeKey = data.locationTypeKey;
            self.locationTypeName = data.locationTypeName;
            self.longitude = data.longitude;
            self.name = data.name;
            if ((typeof data.phone) !== "object") {
                self.phone = data.phone;
            } else {
                self.phone = '';
            }
            self.postalCode = data.postalCode;
            self.region = data.region;
        }
    };

    models.LocationsPagedResponse = function (data) {
        var self = this;
        if (data === undefined) {
            self.locations = [];
            self.pageNum = 0;
            self.itemsPerPage = 0;
            self.totalItems = 0;
            self.totalPages = 0;
        } else {
            self.locations = _.map(data.locations, function (location) {
                return new uLocate.Models.Location(location);
            });
            self.pageNum = data.pageNum;
            self.itemsPerPage = data.itemsPerPage;
            self.totalItems = data.totalItems;
            self.totalPages = Math.ceil(data.totalItems / data.itemsPerPage);
        }
    };

    models.LocationProperty = function (data) {
        var self = this;
        if (data === undefined) {
            self.key = '00000000-0000-0000-0000-000000000000';
            self.propAlias = '';
            self.propData = '';
        } else {
            self.key = data.key;
            self.propAlias = data.propAlias;
            self.propData = data.propData;

        }
    }

}(window.uLocate.Models = window.uLocate.Models || {}));