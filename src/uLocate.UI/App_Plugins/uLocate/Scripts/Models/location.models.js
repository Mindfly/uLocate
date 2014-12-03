(function(models, undefined) {

    models.Address = function(data) {
        var self = this;
        if (data === undefined) {
            self.streetAddress = '';
            self.extendedAddress = '';
            self.locality = '';
            self.region = '';
            self.postalCode = '';
            self.countryName = '';
        } else {
            self.streetAddress = data.streetAddress;
            self.extendedAddress = data.extendedAddress;
            self.locality = data.locality;
            self.region = data.region;
            self.postalCode = data.postalCode;
            self.countryName = data.countryName;
        }
    };

    models.Coordinates = function(data) {
        var self = this;
        if (data === undefined) {
            self.latitude = 0;
            self.longitude = 0;
        } else {
            self.latitude = data.latitude;
            self.longitude = data.longitude;
        }
    };

    models.GetLocationsApiRequst = function(data) {
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
            self.address = new uLocate.Models.Address();
            self.coordinates = new uLocate.Models.Coordinates();
            self.id = '';
            self.name = '';
            self.phone = '';
            self.type = '';
        } else {
            self.address = new uLocate.Models.Address(data.address);
            self.coordinates = new uLocate.Models.Coordinates(data.coordinates);
            self.id = data.id;
            self.name = data.name;
            self.phone = data.phone;
            self.type = data.type;
        }
    };

}(window.uLocate.Models = window.uLocate.Models || {}));