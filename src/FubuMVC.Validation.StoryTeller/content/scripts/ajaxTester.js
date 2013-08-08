var AjaxController = (function() {
  var messages = [];

  return {
    addMessage: function(msg) {
      messages.push(msg);
    },
    allMessages: function() {
      return messages;
    },
    preventSubmission: function() {
      $('#AjaxForm').on('validation:onsubmission', function(event, validation) {
        validation.preventSubmission();
      });
    }
  };
}());

$(function() {
  $('#AjaxForm').on('validation:success', function(event, continuation) {
    AjaxController.addMessage(continuation.message);
  });
});