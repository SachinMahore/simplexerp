$(document).ready(function () {
    ddlAmcFlatNo();
    GetAMCList();
});

var SaveAMCData = function () {
    debugger
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
    var mobile = $("#txtMobile").val();
    var vendorname = $("#txtVendorName").val();
    var amount = $("#txtAmount").val();
    var paidDate = $("#txtPaidDate").val();
    var paidAmount = $("#txtPaidAmount").val();
    var paidBy = $("#txtPaidBy").val();
    var paidMode = $("#txtPaymentMode").val();
    var transactionNo = $("#txtTransactionNo").val();
    var receipt = $("#file1").val();
    var createBy = $("#txtCreateBy").val();
    var createDate = $("#txtCreateDate").val();
    var updateBy = $("#txtUpdateBy").val();
    var updateDate = $("#txtUpdateDate").val();

    $formData.append('Id', id);
    $formData.append('FlatNo', flatNo);
    $formData.append('Title', title);
    $formData.append('MobileNo', mobile);
    $formData.append('VendorName', vendorname);
    $formData.append('Amount', amount);
    $formData.append('PaidDate', paidDate);
    $formData.append('PaidAmount', paidAmount);
    $formData.append('PaidBy', paidBy);
    $formData.append('PaidMode', paidMode);
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
    //    MobileNo: mobile,
    //    VendorName: vendorname,
    //    Amount: amount,
    //    PaidDate: paidDate,
    //    PaidAmount: paidAmount,
    //    PaidBy: paidBy,
    //    PaidMode: paidMode,
    //    TransactionNo: transactionNo,
    //    Receipt: receipt,
    //    CreateBy: createBy,
    //    CreateDate: createDate,
    //    UpdateBy: updateBy,
    //    UpdateDate: updateDate,
    //};
    $.ajax({
        url: "/AMC/SaveAMCData",
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


var ddlAmcFlatNo = function () {
    debugger;
    $.ajax({
        url: "/Resident/GetResident",
        method: "post",
        contentType: "application/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";
            $("#tblAMC tbody").empty();
            html += "<option value='1'>Select Flat No </option>";

            $.each(response.message, function (AMCIndex, elementValue) {
                html += "<option value='" + elementValue.FlatNo + "'>" + elementValue.FlatNo + "</option>";
            });
            $("#txtFlatNo").append(html);
        }
    });
}


var GetAMCList = function () {
    $.ajax({
        url: "/AMC/GetAMCList",
        method: "post",
        contentType: "appliaction/json;charset=utf-8",
        datatype: "json",
        async: false,
        success: function (response) {
            var html = "";
            var totalamount = 0;
            var amount = 0;
            $("#tblAMC tbody").empty();
            $.each(response.model, function (AMCIndex, elementValue) {
                amount = parseInt(amount) + parseInt(elementValue.PaidAmount)
                totalamount = parseInt(totalamount) + parseInt(elementValue.Amount)
                html += "<tr><td>" + elementValue.FlatNo +
                    /*"</td><td>" + elementValue.Id +*/
                    "</td><td>" + elementValue.Title +
                    "</td><td>" + elementValue.VendorName +
                    "</td><td>" + elementValue.MobileNo +
                    "</td><td>" + elementValue.Amount +
                    "</td><td>" + elementValue.PaidAmount +
                    "</td><td>" + elementValue.PaidDate +                  
                    //"</td><td>" + elementValue.PaidBy +
                    //"</td><td>" + elementValue.PaidMode +
                    //"</td><td>" + elementValue.TransactionNo +
                    //"</td><td>" + elementValue.Receipt +
                    "</td><td><button type='button' class='btn btn-warning' value='Detail' onclick='AMCDetail(" + elementValue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";
            });
            $("#tblAMC tbody").append(html);
            $("#lblamount").text("Total Paid Amount = " + amount+" Rs");
            $("#lblTotalamount").text("Total Amount = " + totalamount + " Rs");
           $("#AMCAmount").text( amount + "/-");
        }
    });
}





//DeleteCode
var DeleteAMC = function (Id) {
    var model = { Id: Id };
    $.ajax({
        url: "/AMC/DeleteAMC",
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


var EditAMC = function (Id) {
    var model = { Id: Id };
    $.ajax({

        url: "/AMC/EditAMC",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#hdnId").val(response.model.Id);
            $("#txtFlatNo").val(response.model.FlatNo);
            $("#txtTitle").val(response.model.Title);
            $("#txtMobile").val(response.model.MobileNo);
            $("#txtVendorName").val(response.model.VendorName);
            $("#txtAmount").val(response.model.Amount);
            $("#txtPaidDate").val(response.model.PaidDate);
            $("#txtPaidAmount").val(response.model.PaidAmount);
            $("#txtPaidBy").val(response.model.PaidBy);
            $("#txtPaymentMode").val(response.model.PaidMode);
            $("#txtTransactionNo").val(response.model.TransactionNo);
            $("#txtReceipt").val(response.model.Receipt);
            $("#txtCreateBy").val(response.model.CreateBy);
            $("#txtCreateDate").val(response.model.CreateDate);
            $("#txtUpdateBy").val(response.model.UpdateBy);
            $("#txtUpdateDate").val(response.model.UpdateDate);

        }
    });
}


//DetailCode

var AMCDetail = function (Id) {
    debugger;
    var model = { Id: Id }
    $.ajax({
        url: "/AMC/DetailAMC",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#AMCModal").modal('show');

            $("#DetailAMC").empty();

            var html = "";
            html += "<div class='row'>";
            html += "<div class='col-sm-6'>";
            html += "<b>Id:</b>&nbsp&nbsp&nbsp<span>" + response.model.Id + "</span>";
            html += "</br>";
            html += "<b>Flat Number:</b>&nbsp&nbsp&nbsp<span>" + response.model.FlatNo + "</span>";
            html += "</br>";
            html += "<b>Title:</b>&nbsp&nbsp&nbsp<span>" + response.model.Title + "</span>";
            html += "</br>";
            html += "<b>VendorName:</b>&nbsp&nbsp&nbsp<span>" + response.model.VendorName + "</span>";
            html += "</br>";
            html += "<b>MobileNo:</b>&nbsp&nbsp&nbsp<span>" + response.model.MobileNo + "</span>";
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

            $("#DetailAMC").append(html);
        }
    });
};



  
