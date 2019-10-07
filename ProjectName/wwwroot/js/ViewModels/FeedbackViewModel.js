function FeedbackViewModel(status, message, description, modelStateArray) {
    var self = this;

    self.Status = status; // This is values such as "success", error", "critical"
    self.Message = message; // Primary Message that gets displayed
    self.Description = description; // Secondary message which gets displayed, should contain the debug info.
    self.ModelState = modelStateArray; // Contains an array of validation errors on the current form.
}