var AjaxController = (function() {
  var messages = [];

  return {
    addMessage: function(msg) {
      messages.push(msg);
    },
    allMessages: function() {
      return messages;
    }
  };
}());

$(function() {
  $('#AjaxForm').on('validation:success', function(event, continuation) {
    AjaxController.addMessage(continuation.message);
  });
});