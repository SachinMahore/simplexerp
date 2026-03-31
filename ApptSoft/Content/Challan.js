$(document).ready(function () {
    //GetVisitors();
});
var SaveChallan = function () {
    debugger;

    var $formData = new FormData();

    // Read values
    var challanID = $("#hdnChallanID").val();
    var challanNo = $("#txtChallanNo").val();
    var date = $("#txtDate").val();
    var ashUtilization = $("#txtAshUtilization").val();
    var partyName = $("#txtPartyName").val();
    var partyLocation = $("#txtPartyLocation").val();
    var vehicleNo = $("#txtVehicleNo").val();
    var vehicleType = $("#txtVehicleType").val();
    var noOfWheels = $("#txtNoOfWheels").val();
    var gatePassNo = $("#txtGatePassNo").val();
    var gatePassValidity = $("#txtGatePassValidity").val();
    var commodityType = $("#txtCommodityType").val();
    var pickupLocation = $("#txtPickupLocation").val();
    var unloadLocation = $("#txtUnloadLocation").val();

    // --------------------- Validation ----------------------
    if (challanNo === "") {
        alert("Challan No is required.");
        $("#txtChallanNo").focus();
        return;
    }
    if (date === "") {
        alert("Date is required.");
        $("#txtDate").focus();
        return;
    }
    if (partyName === "") {
        alert("Party Name is required.");
        $("#txtPartyName").focus();
        return;
    }
    if (vehicleNo === "") {
        alert("Vehicle No is required.");
        $("#txtVehicleNo").focus();
        return;
    }
    if (commodityType === "") {
        alert("Commodity Type is required.");
        $("#txtCommodityType").focus();
        return;
    }

    // --------------------- Append Form Data ----------------------
    $formData.append("ChallanID", challanID);
    $formData.append("ChallanNo", challanNo);
    $formData.append("Date", date);
    $formData.append("AshUtilizationFor", ashUtilization);
    $formData.append("PartyName", partyName);
    $formData.append("PartyLocation", partyLocation);
    $formData.append("VehicleNo", vehicleNo);
    $formData.append("VehicleType", vehicleType);
    $formData.append("NoOfWheels", noOfWheels);
    $formData.append("GatePassNo", gatePassNo);
    $formData.append("GatePassValidity", gatePassValidity);
    $formData.append("CommodityType", commodityType);
    $formData.append("AshPickupLocation", pickupLocation);
    $formData.append("AshUnloadLocation", unloadLocation);

    // --------------------- AJAX Call ----------------------
    $.ajax({
        url: "/Challan/SaveChallan",
        method: "POST",
        data: $formData,
        contentType: false,
        processData: false,
        async: false,
        success: function (response) {
            alert(response.Message || response);
            //GetChallans(); // Refresh data list
        },
        error: function (response) {
            alert("Error: " + response.responseText);
        }
    });
};
