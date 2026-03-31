$(document).ready(function () {
    ddlResidentFlatno();

    GetCorpusFund();
    $("#txtPaidAmount").keyup(function () {
        var total = 0;
        var p = Number($("#txtAmount").val());
        var q = Number($("#txtPaidAmount").val());
        var total = p - q;
        $("#txtBalanceAmount").val(total);
    });
});


var SaveCorpusFund = function () {
    debugger;
    var id = $("#hdnId").val();
    var flatno = $("#txtFlatNo").val();
    var amount = $("#txtAmount").val();
    var paidamount = $("#txtPaidAmount").val();
    var balanceamount = $("#txtBalanceAmount").val();
    var createby = $("#txtCreateBy").val();
    var createdate = $("#txtCreateDate").val();
    var updateby = $("#txtUpdateBy").val();
    var updatedate = $("#txtUpdateDate").val();
   

    var model = {
        Id: id,
        FlatNo: flatno,
        Amount: amount,
        PaidAmount: paidamount,
        BalanceAmount: balanceamount,
        CreateBy: createby,
        CreateDate: createdate,
        UpdateBy: updateby,
        UpdateDate: updatedate

    };
    $.ajax({
        url: "/CorpusFund/SaveCorpusFund",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {

            alert(response.message);
            GetResident();
        },
        error: function (response) {
            alert(response.message);
        }
    });
}
var ddlResidentFlatno = function () {
    debugger;
    $.ajax({
        url: "/Resident/GetResident",
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var html = "";
            $("#tblCorpusFund tbody").empty();
            html += "<option value='1'>Select Flat No </option>";

            $.each(response.message, function (CorpusIndex, elementvalue) {

                html += "<option value='" + elementvalue.FlatNo + "'>" + elementvalue.FlatNo + "</option>";
            });
            $("#txtFlatNo").append(html);
        }

    });

}
var GetCorpusFund = function () {
    debugger;
    $.ajax({
        url: "/CorpusFund/GetCorpusFund",
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var html = "";
            var totalamount = 0;
            var Paidamount = 0;
            var balanceamount = 0;
            $("#tblCorpusFund tbody").empty();
            $.each(response.message, function (CorpusIndex, elementvalue) {
                totalamount = parseInt(totalamount) + parseInt(elementvalue.Amount)
                Paidamount = parseInt(Paidamount) + parseInt(elementvalue.PaidAmount)
                balanceamount = parseInt(balanceamount) + parseInt(elementvalue.BalanceAmount)
                html += "<tr><td>"/* + elementvalue.Id + "</td><td>"*/
                    + elementvalue.FlatNo + "</td><td>"
                    + elementvalue.ResidentName + "</td><td>"
                    + elementvalue.PaidAmount + "</td><td>"
                    + elementvalue.BalanceAmount + "</td><td>"
                    + elementvalue.CreateDate + "</td><td><button type='button' class='btn btn-warning' value='Detail' onclick='CorpusFundDetail(" + elementvalue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";
            });
            $("#tblCorpusFund tbody").append(html);
            $("#lblamount").text("Total Amount = " + totalamount + " Rs");
            $("#lblPaidamount").text("Total Paid Amount = " + Paidamount + " Rs");
            $("#lblBalanceamount").text("Total Balance Amount = " + balanceamount + " Rs");
            $("#CorpusFundAmount").text(Paidamount + "/-");
        } 

    });
    
}

var EditCorpusFund = function (Id) {
    debugger;
    var model = { Id: Id };
    $.ajax({
        url: "/CorpusFund/EditCorpusFund",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            
            $("#hdnId").val(response.model.Id);
            $("#txtFlatNo").val(response.model.FlatNo);           
            $("#txtAmount").val(response.model.Amount);
            $("#txtBalanceAmount").val(response.model.BalanceAmount);
            $("#txtPaidAmount").val(response.model.PaidAmount);
            $("#txtCreateDate").val(response.model.CreateDate);
            $("#txtUpdateDate").val(response.model.UpdateBy);
            $("#txtUpdateDate").val(response.model.UpdateDate);
            
        }
    });
}
var DeleteCorpusFund = function (Id) {
    var model = { Id: Id };
    $.ajax({
        url: "/CorpusFund/DeleteResident",
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


//DetailCode

var CorpusFundDetail = function (Id) {
    debugger;
    var model = { Id: Id }
    $.ajax({
        url: "/CorpusFund/DetailCopusFund",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#CorpusFundModal").modal('show');

            $("#DetailCorpusFund").empty();

            var html = "";
            html += "<div class='row'>";
            html += "<div class='col-sm-6'>";
            html += "<b>Id:</b>&nbsp&nbsp&nbsp<span>" + response.model.Id + "</span>";
            html += "</br>";
            html += "<b>Flat Number:</b>&nbsp&nbsp&nbsp<span>" + response.model.FlatNo + "</span>";
            html += "</br>";                    
            html += "<b>Amount:</b>&nbsp&nbsp&nbsp<span>" + response.model.Amount + "</span>";
            html += "</br>";
            html += "<b>PaidAmount:</b>&nbsp&nbsp&nbsp<span>" + response.model.PaidAmount + "</span>";
            html += "</br>";
            html += "<b>BalanceAmount:</b>&nbsp&nbsp&nbsp<span>" + response.model.BalanceAmount + "</span>";
            html += "</br>";
           
            html += "</div>";
            html += "</div>";

            $("#DetailCorpusFund").append(html);
        }
    });
};