function contactlistload(loginuserid, contactsearch) {
    $.ajax({
        type: 'GET',
        url: '/Controller/Inbox/contactlist?loginuserid=' + loginuserid,
        data: {
            contact: contactsearch
        },
        success: function (result) {
            $('#inboxmycontacts').html(result);
        },
        error: function () {
            alert('Error loading contactlistload.');
        }
    });
}


function readstatusUpdate(chatid) {
    $.ajax({
        type: 'POST',
        url: '/Controller/Inbox/inboxmessageupdate?chatid=' + chatid,
        success: function (result) {
            // Handle the success response here
            // For example, you can update the UI or perform other actions.
            // $('#inboxmycontacts').html(result);
        },
        error: function () {
            // Handle the error here
            alert('Error loading readstatusUpdate.');
        }
    });
}


function messagelistload(chatid, loginuserid, pagenumber) {
    $.ajax({
        type: 'GET',
        url: '/Controller/Inbox/messagelist?chatid=' + chatid + '&loginuserid=' + loginuserid + '&pagenumber=' + pagenumber,

        success: function (result) {
            $('#InboxMessageContainer').html(result);


        },
        error: function () {
            alert('Error loading messagelistload.');
        }
    });
}


function messagelistloadhistory(chatid, loginuserid, pagenumber) {
    $.ajax({
        type: 'GET',
        url: '/Controller/Inbox/messagelist?chatid=' + chatid + '&loginuserid=' + loginuserid + '&pagenumber=' + pagenumber,

        success: function (result) {
            $('#InboxMessageContainerHistory').prepend(result);
        },
        error: function () {
            alert('Error loading messagelistload history.');
        }
    });
}



function userview(ProfileGuid) {
    $.ajax({
        type: 'GET',
        url: '/Controller/Inbox/UserView?ProfileGuid=' + ProfileGuid,

        success: function (result) {
            $('#dvUserViewContainer').html(result);
        },
        error: function (err) {
            console.log(err);
        }
    });
}
