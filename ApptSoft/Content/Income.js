$(document).ready(function () {
    GetIncome();
    GetIncomeAmount();
});


var SaveIncome = function () {
    debugger;
    var id = $("#hdnId").val();
    var month = $("#txtMonth").val();
    var year = $("#txtYear").val();
    var incomefrom = $("#txtIncomeFrom").val();
    var amount = $("#txtAmount").val();
    var addeddate = $("#txtAddedDate").val();
    var createby = $("#txtCreateBy").val();
    var createdate = $("#txtCreateDate").val();
    var updateby = $("#txtUpdateBy").val();
    var updatedate = $("#txtUpdateDate").val();


    var model = {
        Id: id,
        Month: month,
        Year: year,
        IncomeFrom: incomefrom,
        Amount: amount,
        AddedDate: addeddate,
        CreateBy: createby,
        CreateDate: createdate,
        UpdateBy: updateby,
        UpdateDate: updatedate

    };
    $.ajax({
        url: "/Income/SaveIncome",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            alert(response.Message);
            CorpusFundAmount();
            
        },
        error: function (response) {
            alert(response.Message);
        }
    });
}


var GetIncome = function () {
    debugger;
    $.ajax({
        url: "/Income/GetIncome?Year=" + $("#txtYear1").val() + "&month=" + $("#txtMonth1").val(),
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var html = "";
            var amount = 0;
            $("#tblIncome tbody").empty();
            $.each(response.Message, function (IncomeIndex, elementvalue) {
                amount = parseInt(amount) + parseInt(elementvalue.Amount)
                html += "<tr><td>"
                    + elementvalue.Month + "</td><td>"
                   /* + elementvalue.Id + "</td><td>"*/
                    + elementvalue.Year + "</td><td>"
                    + elementvalue.IncomeFrom + "</td><td>"
                    + elementvalue.Amount + "</td><td>"
                    + elementvalue.AddedDate + "</td><td><button type='button' class='btn btn-warning' value='Detail' onclick='IncomeDetail(" + elementvalue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";
                 
            });
            $("#tblIncome tbody").append(html);
            $("#lblamount").text("Total Income = " + amount + " Rs");
           
            
        }

    });

}




var GetIncomeAmount = function () {
    debugger;
    $.ajax({
        url: "/Income/GetIncome",
        method: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            var html = "";
            var amount = 0;
            $("#tblIncome tbody").empty();
            $.each(response.Message, function (IncomeIndex, elementvalue) {
                amount = parseInt(amount) + parseInt(elementvalue.Amount)
                html += "<tr><td>"
                    + elementvalue.Month + "</td><td>"
                    /* + elementvalue.Id + "</td><td>"*/
                    + elementvalue.Year + "</td><td>"
                    + elementvalue.IncomeFrom + "</td><td>"
                    + elementvalue.Amount + "</td><td>"
                    + elementvalue.AddedDate + "</td><td><button type='button' class='btn btn-warning' value='Detail' onclick='IncomeDetail(" + elementvalue.Id + ")'><i class='bi bi-eye-fill'></i></button></td></tr>";

            });
            $("#IncomeAmount").text(amount +"/-");

        }

    });

}


var EditIncome = function (Id) {
    debugger;
    var model = { Id: Id };
    $.ajax({
        url: "/Income/EditIncome",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {

            var sdate = response.model.AddedDate.split("-");
            var addeddate = sdate[2] + "-" + sdate[1] + "-" + sdate[0];

            $("#hdnId").val(response.model.Id);
            $("#txtMonth").val(response.model.Month);
            $("#txtYear").val(response.model.Year);
            $("#txtIncomeFrom").val(response.model.IncomeFrom);
            $("#txtMobileNo").val(response.model.MobileNo);
            $("#txtAmount").val(response.model.Amount);
            $("#txtAddedDate").val(addeddate);
            $("#txtCreateBy").val(response.model.CreateBy);
            $("#txtCreateDate").val(response.model.CreateDate);
            $("#txtUpdateDate").val(response.model.UpdateBy);
            $("#txtUpdateDate").val(response.model.UpdateDate);
           

        }
    });
}
var DeleteIncome = function (Id)
{
    debugger
    var model = { Id:Id };
    $.ajax({
        url: "/Income/DeleteIncome",
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

var MonthChangeEvent = function () {
    $("#txtYear1").val("");
    GetIncome();
}
var yearchangeenven = function () {

    if ($("#txtMonth1").val() == "") {
        alert("Please Select Month!");
        $("#txtYear1").val("");
        return false();
    }

    GetIncome();
}

//DetailCode

var IncomeDetail = function (Id) {
    debugger;
    var model = { Id: Id }
    $.ajax({
        url: "/Income/DetailIncome",
        method: "post",
        data: JSON.stringify(model),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $("#IncomeModal").modal('show');

            $("#DetailIncome").empty();

            var html = "";
            html += "<div class='row'>";
            html += "<div class='col-sm-6'>";
            html += "<b>Id:</b>&nbsp&nbsp&nbsp<span>" + response.model.Id + "</span>";
            html += "</br>";
            html += "<b>Month:</b>&nbsp&nbsp&nbsp<span>" + response.model.Month + "</span>";
            html += "</br>";
            html += "<b>Year:</b>&nbsp&nbsp&nbsp<span>" + response.model.Year + "</span>";
            html += "</br>";
            html += "<b>Income From:</b>&nbsp&nbsp&nbsp<span>" + response.model.IncomeFrom + "</span>";
            html += "</br>";
            html += "<b>MobileNo:</b>&nbsp&nbsp&nbsp<span>" + response.model.MobileNo + "</span>";
            html += "</br>";
            html += "<b>Amount:</b>&nbsp&nbsp&nbsp<span>" + response.model.Amount + "</span>";
            html += "</br>";
            html += "<b>Added Date:</b>&nbsp&nbsp&nbsp<span>" + response.model.AddedDate + "</span>";
            html += "</br>";
            html += "</div>";
            html += "</div>";

            $("#DetailIncome").append(html);
        }
    });
};

