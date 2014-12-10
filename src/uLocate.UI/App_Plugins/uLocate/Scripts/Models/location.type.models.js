(function (models, undefined) {

    models.LocationTypeProperty = function(data) {
        var self = this;
        if (data === undefined) {
            self.alias = '';
            self.databaseType = '';
            self.dataTypeId = '';
            self.locationTypeKey = '';
            self.key = '';
            self.name = '';
            self.propertyEditorAlias = '';
            self.sortOrder = '';
        } else {
            self.alias = data.alias;
            self.databaseType = data.databaseType;
            self.dataTypeId = data.dataTypeId;
            self.locationTypeKey = data.locationTypeKey;
            self.key = data.key;
            self.name = data.name;
            self.propertyEditorAlias = data.propertyEditorAlias;
            self.sortOrder = data.sortOrder;
        }
    }

    models.LocationType = function (data) {
        var self = this;
        if (data === undefined) {
            self.description = '';
            self.icon = '';
            self.key = '';
            self.name = '';
            self.properties = [new uLocate.Models.LocationTypeProperty()];
        } else {
            self.description = data.description;
            self.icon = data.icon;
            self.key = data.key;
            self.name = data.name;
            if (data.properties) {
                self.properties = _.map(data.properties, function(property) {
                    return new uLocate.Models.LocationTypeProperty(property);
                });
            } else {
                self.properties = [];
            }
        }
    };

}(window.uLocate.Models = window.uLocate.Models || {}));