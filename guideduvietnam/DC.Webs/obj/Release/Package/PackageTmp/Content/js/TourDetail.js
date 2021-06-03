var viewModel = new function () {
    var self = this;
    // Thông tin chung
    var language = data.Language;
    var itemInfo = data.CustomizeTourModel;
    self.HotelItems = ko.observableArray(data.HotelItems != null ? data.HotelItems : []);    
    self.Name = ko.observable('').extend({ required: { message: language == 'en' ? 'required' : 'requis' } });
     
    self.YourName = ko.observable('').extend({ required: { message: language == 'en' ? 'required' : 'requis' } });
    self.Email = ko.observable('').extend({ required: { message: language == 'en' ? 'required' : 'requis' } });
    self.Nationality = ko.observable('');
    self.Telephone = ko.observable('').extend({ required: { message: language == 'en' ? 'required' : 'requis' } });
    self.ArrivalDate = ko.observable('');
    self.DepatureDate = ko.observable('');
    self.PersonNumber = ko.observable('1');
    self.Lunch = ko.observable(false);
    self.Dinner = ko.observable(false);
    self.Flight = ko.observable('Yes');
    self.Message = ko.observable('');
    self.Status = ko.observable(false);
    self.MessageError = ko.observable('');
    self.TourName = ko.observable(itemInfo.TourName);
    // Submit
    self.AddData = function () {
        itemInfo.TourName = self.TourName();
        itemInfo.Name = self.Name();
        itemInfo.YourName = self.YourName();
        itemInfo.Email = self.Email();
        itemInfo.Nationality = self.Nationality();
        itemInfo.Telephone = self.Telephone();
        itemInfo.ArrivalDate = self.ArrivalDate();
        itemInfo.DepatureDate = self.DepatureDate();
        itemInfo.Lunch = self.Lunch();
        itemInfo.Dinner = self.Dinner();
        itemInfo.Flight = self.Flight();
        itemInfo.Message = self.Message();
        itemInfo.PersonNumber = self.PersonNumber();
        
        var hotelcheckboxValues = [];
        $('input:checkbox[name=Hotel]:checked').each(function () {
            hotelcheckboxValues.push($(this).val());            
        });
        itemInfo.HotelId = '' + hotelcheckboxValues + '';
    }
    self.Submit = function () {
        var captcha = grecaptcha.getResponse();
        self.AddData();
        if (viewModel.errors().length === 0 && captcha!=='') {
            // Submit Form
            openProcess();
            $.ajax({
                async: false,
                type: "POST",
                url: "/Tours/CustomizeTour",
                data: JSON.stringify({ item: itemInfo, response: captcha}),
                dataType: "json",
                contentType: 'application/json; charset=UTF-8',
                success: function (data, textStatus, jqXHR) {
                    hideProcess();
                    self.Status(true);
                    if (language === "en" && data !== 'ERROR') {
                        self.MessageError('Your email was sent to Vietnam Travel Motorbike. We will answer you in 24h. Thank you very much and see you again!');
                    }
                    if (language === "fr" && data !== 'ERROR') {
                        self.MessageError('Votre é – mail a été envoyé à Voyage Vietnam Moto. Nous allons vous y répondre en 24h. Merci beaucoup et à très bien tôt!');
                    }

                },
                error: function (xhr, status, error) {
                    hideProcess();
                    showPopupError();
                }
            });
        }
        else {
            self.Status(true);
            self.MessageError(language == 'en' ? 'Please confirm the fields and submit it again' : 'S\'il vous plaît confirmer les champs et le soumettre à nouveau');
            viewModel.errors.showAllMessages();
        }
    }
};

viewModel.errors = ko.validation.group(viewModel);
ko.validation.rules.pattern.message = 'Invalid.';
ko.applyBindings(viewModel, document.getElementById("CustomizeTour"));