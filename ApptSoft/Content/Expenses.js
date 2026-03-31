$(document).ready(function () {
    currentMontYear();
    ddlGetExpenses();
    GetExpensesList();


});

var SaveExpensesData = function () {
    debugger;

    $formData = new FormData();
    var Image = document.getElementById('file1');
    if (Image.files.length > 0) {
        for (var i = 0; i < Image.files.length; i++) {
            $formData.append('file1-' + i, Image.files[i]);
        }
    }

    var id = $("#hdnId").val();
    var flatNo = $("#txtFlatNo").val();
    var title = $("#txtTitle").val();
    var month = $("#txtMonth").val();
    var year = $("#txtYear").val();
    var amount = $("#txtAmount").val();
    var paidDate = $("#txtPaidDate").val();
    var paidAmount = $("#txtPaidAmount").val();
    var paidBy = $("#txtPaidBy").val();
    var paymentMode = $("#txtPaymentMode").val();
    var transactionNo = $("#txtTransactionNo").val();
    var receipt = $("#file1").val();
    var createBy = $("#txtCreateBy").val();
    var createDate = $("#txtCreateDate").val();
    var updateBy = $("#txtUpdateBy").val();
    var updateDate = $("#txtUpdateDate").val();
    // Check if any field is empty
    if (!transactionNo || !title || !month || !year || !amount || !paidAmount) {
        alert("All fields are required. Please fill in all details.");
        return false; // Stop form submission
    }

    $formData.append('Id', id);
    $formData.append('FlatNo', flatNo);
    $formData.append('Title', title);
    $formData.append('Month', month);
    $formData.append('Year', year);
    $formData.append('Amount', amount);
    $formData.append('PaidDate', paidDate);
    $formData.append('PaidAmount', paidAmount);
    $formData.append('PaidBy', paidBy);
    $formData.append('PaymentMode', paymentMode);
    $formData.append('TransactionNo', transactionNo);
    $formData.append('Receipt', receipt);
    $formData.append('CreateBy', createBy);
    $formData.append('CreateDate', createDate);
    $formData.append('UpdateBy', updateBy);
    $formData.append('UpdateDate', updateDate);


    //var model = {
    //    Id: id,
    //    FlatNo: flatNo,
    //    Title: title,
    //    Month: month,
    //    Year: year,
    //    Amount: amount,
    //    PaidDate: paidDate,
    //    PaidAmount: paidAmount,
    //    PaidBy: paidBy,
    //    PaymentMode: paymentMode,
    //    TransactionNo: transactionNo,
    //    Receipt: receipt,
    //    CreateBy: createBy,
    //    CreateDate: createDate,
    //    UpdateBy: updateBy,
    //    UpdateDate: updateDate,
    //};
    $.ajax({
        url: "/Expenses/SaveExpensesData",
        method: "Post",
        data: $formData,
        contentType: "application/json;charset=utf-8",
        contentType: false,
        processData: false,
        async: false,

        success: function (response) {
            location.reload();
            alert(response.message);
        }
    });

}




var ddlGetExpenses = function () {
    debugger;
    $.ajax({
        url: "/Resident/GetResident",
        method: "post",
        contentType: "appliaction/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";
            $("#tblExpens tbody").empty();
            html += "<option value='1'>Select Flat No </option>";

            $.each(response.message, function (index, elementValue) {
                html += "<option value='" + elementValue.FlatNo + "'>" + elementValue.FlatNo + "</option>";

            });
            $("#txtFlatNo").append(html);
        }
    });
}


var GetExpensesList = function () {
    //debugger;
    $.ajax({
        url: "/Expenses/GetExpensesList",
        method: "post",
        contentType: "appliaction/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";
            var totalamount = 0;
            var Paidamount = 0;

            $("#tblExpens tbody").empty();
            $.each(response.model, function (index, elementValue) {
                totalamount = parseInt(totalamount) + parseInt(elementValue.Amount)
                Paidamount = parseInt(Paidamount) + parseInt(elementValue.PaidAmount)

                html += "<tr><td><b>" + elementValue.Title +
                    "</b></td><td>" + elementValue.Month +
                    "</td><td>" + elementValue.Year +
                    "</td><td>" + elementValue.Amount +
                    "</td><td>" + elementValue.PaidAmount +
                    "</td><td>" + elementValue.PaidDate +
                    "</td><td>" + elementValue.TransactionNo +
                    //"</td><td>" + elementValue.PaymentMode +
                    //"</td><td>" + elementValue.TransactionNo +
                    /* "</td><td><img src='../Content/Img/" + elementValue.Receipt + "'style='max-width:80px;max-height:80px;'/> " +*/
                    "</td><td><button type='button' class='btn btn-warning' value='Detail' onclick='ExpensesDetail(" + elementValue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";

            });

            $("#tblExpens tbody").append(html);
            $("#lblamount").text("Total Amount = " + totalamount + " Rs");
            $("#lblPaidamount").text("Total Paid Amount = " + Paidamount + " Rs");
            $("#lblPaidamount1").text(Paidamount + "/-");



        }
    });
}






//DeleteCode
var DeleteExpenses = function (Id) {
    var model = { Id: Id };
    $.ajax({
        url: "/Expenses/DeleteExpenses",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            alert("Record Deleted Successfully");
        }
    });
}

//EditCode


var EditExpenses = function (Id) {
    var model = { Id: Id };
    $.ajax({

        url: "/Expenses/EditExpenses",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#hdnId").val(response.model.Id);
            $("#txtFlatNo").val(response.model.FlatNo);
            $("#txtTitle").val(response.model.Title);
            $("#txtMonth").val(response.model.Month);
            $("#txtYear").val(response.model.Year);
            $("#txtAmount").val(response.model.Amount);
            $("#txtPaidDate").val(response.model.PaidDate);
            $("#txtPaidAmount").val(response.model.PaidAmount);
            $("#txtPaidBy").val(response.model.PaidBy);
            $("#txtPaymentMode").val(response.model.PaymentMode);
            $("#txtTransactionNo").val(response.model.TransactionNo);
            $("#txtReceipt").val(response.model.Receipt);
            $("#txtCreateBy").val(response.model.CreateBy);
            $("#txtCreateDate").val(response.model.CreateDate);
            $("#txtUpdateBy").val(response.model.UpdateBy);
            $("#txtUpdateDate").val(response.model.UpdateDate);

        }
    });
}

var currentMontYear = function () {
    var currentDate = new Date();
    $("#txtYear").val(currentDate.getFullYear());

    var currentMonth;
    switch (currentDate.getMonth()) {
        case 0: currentMonth = "January";
            break;
        case 1: currentMonth = "February";
            break;
        case 2: currentMonth = "March";
            break;
        case 3: currentMonth = "April";
            break;
        case 4: currentMonth = "May";
            break;
        case 5: currentMonth = "June";
            break;
        case 6: currentMonth = "July";
            break;
        case 7: currentMonth = "August";
            break;
        case 8: currentMonth = "September";
            break;
        case 9: currentMonth = "October";
            break;
        case 10: currentMonth = "November";
            break;
        case 11: currentMonth = "December";
            break;
    }
    $("#txtMonth").val(currentMonth);
}

//DetailCode
var ExpensesDetail = function (Id) {
    debugger;
    var model = { Id: Id }
    $.ajax({
        url: "/Expenses/DetailExpenses",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#ExpensesModal").modal('show');

            $("#DetailExpenses").empty();

            var html = "";
            html += "<div class='row'>";
            html += "<div class='col-sm-6'>";
            html += "<b>Id:</b>&nbsp&nbsp&nbsp<span>" + response.model.Id + "</span>";
            html += "</br>";
            html += "<b>Flat Number:</b>&nbsp&nbsp&nbsp<span>" + response.model.FlatNo + "</span>";
            html += "</br>";
            html += "<b>Title:</b>&nbsp&nbsp&nbsp<span>" + response.model.Title + "</span>";
            html += "</br>";
            html += "<b>Month:</b>&nbsp&nbsp&nbsp<span>" + response.model.Month + "</span>";
            html += "</br>";
            html += "<b>Year:</b>&nbsp&nbsp&nbsp<span>" + response.model.Year + "</span>";
            html += "</br>";
            html += "<b>Amount:</b>&nbsp&nbsp&nbsp<span>" + response.model.Amount + "</span>";
            html += "</br>";
            html += "<b>PaidAmount:</b>&nbsp&nbsp&nbsp<span>" + response.model.PaidAmount + "</span>";
            html += "</br>";
            html += "<b>PaymentMode:</b>&nbsp&nbsp&nbsp<span>" + response.model.PaymentMode + "</span>";
            html += "</br>";
            html += "<b>TransactionNo:</b>&nbsp&nbsp&nbsp<span>" + response.model.TransactionNo + "</span>";
            html += "</br>";
            html += "<b>PaidBy:</b>&nbsp&nbsp&nbsp<span>" + response.model.PaidBy + "</span>";
            html += "</br>";
            html += "<b>PaidDate:</b>&nbsp&nbsp&nbsp<span>" + response.model.PaidDate + "</span>";
            html += "</br>";
            html += "</div>";
            html += "<div class='col-sm-6'>";
            html += "<b>Receipt:</b>&nbsp&nbsp&nbsp<span><img src='../Content/Img/" + response.model.Receipt + "'style='width:150px;height:180px;'/> </span > ";
            html += "</br>";
            html += "</div>";
            html += "</div>";


            $("#DetailExpenses").append(html);
        }
    });
};