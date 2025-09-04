$(function () {
    GetAllRelationships();

    $(document).off("click", ".settingAllTab");
    $(document).on("click", ".settingAllTab", function (e) {
        e.preventDefault();
        var tab = $(this).attr("data-tab");
        if (tab === "source") {
            GetAllRelationships();
        }
        else if (tab === "event") {
            GetAllEventTypes();
        }
        else if (tab === "severity") {
            GetAllSeverity();
        }
        else if (tab === "status") {
            GetAllStatus();
        }
    });


    $(document).off("click", ".btnAddNewSource");
    $(document).on("click", ".btnAddNewSource", function (e) {
        e.preventDefault();
        AddRelationships();
    });

    $(document).off("click", ".cancelSource");
    $(document).on("click", ".cancelSource", function (e) {
        e.preventDefault();
        $("#addSource").empty().html('');
        $('li.active').trigger('click')
    });

    $(document).off("click", ".saveSource");
    $(document).on("click", ".saveSource", function (e) {
        e.preventDefault();
        var isValid = true;

        $("#saveSourceDiv").find("input[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            if (value === "") {
                isValid = false;
                showError($field);
            }
            else {
                clearError($field);
            }
        });
        if (isValid) {
            SaveRelationships();
        }
    });

    $(document).off("click", ".editSource");
    $(document).on("click", ".editSource", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetRelationshipById(id);
    });

    $(document).off("click", ".deleteSource");
    $(document).on("click", ".deleteSource", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        DeleteSourceItem(id);
    });


    $(document).off("click", ".btnAddNewEvent");
    $(document).on("click", ".btnAddNewEvent", function (e) {
        e.preventDefault();
        AddEventType();
    });


    $(document).off("click", ".cancelEventType");
    $(document).on("click", ".cancelEventType", function (e) {
        e.preventDefault();
        $("#addEventType").empty().html('');
        $('li.active').trigger('click')
    });

    $(document).off("click", ".saveEventType");
    $(document).on("click", ".saveEventType", function (e) {
        e.preventDefault();
        var isValid = true;

        $("#saveEventTypeDiv").find("input[data-val-required], textarea[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            if (value === "") {
                isValid = false;
                showError($field);
            } else {
                clearError($field);
            }
        });

        // ✅ only run once after validation check
        if (isValid) {
            SaveEventType();
        }
    });


    $(document).off("click", ".editEventType");
    $(document).on("click", ".editEventType", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetEventTypeById(id);
    });

    $(document).off("click", ".deleteEventType");
    $(document).on("click", ".deleteEventType", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");

        //e.preventDefault();
        DeleteEventTypeItem(id);
    });

    // Show error: red border + message
    function showError($field) {
        $field.css("border", "1px solid red");
    }

    // Clear error: reset border + remove message
    function clearError($field) {
        $field.css("border", ""); // reset to default
        $field.siblings(".field-validation-error").remove();
    }
})

// Start Source
async function GetAllRelationships() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetAllRelationships", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        const content = await response.text();
        $("#sourceList").empty().html(content);

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function AddRelationships() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/AddRelationships", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        const content = await response.text();
        $("#addSource").empty().html(content);

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function GetRelationshipById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetRelationshipById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        const content = await response.text();
        $("#addSource").empty().html(content);

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function DeleteRelationshipById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/DeleteRelationshipById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        SwalSuccessAlert("Source deleted successfully!");
        GetAllRelationships();

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function SaveRelationships() {
    try {

        let form = [];
        let formData = new FormData();
        let obj = $("#NewSourceForm")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        });


        showLoader($(".setting"));

        //console.log(formData);
        console.log(form);

        // Send request
        let response = await fetch("/Settings/SaveRelation", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            $("#addSource").html("");
            SwalSuccessAlert(result.data);
            GetAllRelationships();
        } else {
            SwalErrorAlert(result.message || "Failed to save relation.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving relation!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}
function DeleteSourceItem(id) {   // <-- accept id
    let confirmBtnText = "Yes, delete it!";
    let cancelBtnText = "No, cancel!";

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success me-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.isConfirmed) {   // ✅ correct way
            DeleteRelationshipById(id);
        }
    });
}

// End Source


// Start Event Type
async function GetAllEventTypes() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetAllEventTypes", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        const content = await response.text();
        $("#eventTypeList").empty().html(content);

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function AddEventType() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/AddEventType", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        const content = await response.text();
        $("#addEventType").empty().html(content);

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function GetEventTypeById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetEventTypeById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        const content = await response.text();
        $("#addEventType").empty().html(content);

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function DeleteEventTypeById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/DeleteEventTypeById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        SwalSuccessAlert("Event Type deleted successfully!");
        GetAllEventTypes();

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function SaveEventType() {
    try {

        let form = [];
        let formData = new FormData();
        let obj = $("#NewEventTypeForm")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        });


        showLoader($(".setting"));

        //console.log(formData);
        console.log(form);

        // Send request
        let response = await fetch("/Settings/SaveEventType", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            $("#addEventType").html("");
            SwalSuccessAlert(result.data);
            GetAllEventTypes();
        } else {
            SwalErrorAlert(result.message || "Failed to save event type.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving event type!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}
function DeleteEventTypeItem(id) {   // <-- accept id
    let confirmBtnText = "Yes, delete it!";
    let cancelBtnText = "No, cancel!";

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success me-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.isConfirmed) {   // ✅ correct way
            DeleteEventTypeById(id);
        }
    });
}

// End Event Type