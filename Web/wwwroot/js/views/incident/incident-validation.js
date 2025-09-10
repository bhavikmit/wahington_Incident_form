var sendCommunication = [];
var index = 0;
$(function () {

    let currentStep = 1;
    const totalSteps = 5;

    //$("#div_ValidationDtl").empty().html("");

    GetValidationsDetail($("#hdn_Id").val());

    //GetValidationsList();

    //$(document).off("click", ".validateIncident");
    //$(document).on("click", ".validateIncident", function (e) {
    //    var id = $(this).attr("data-incident-id");
    //    $("#div_ValidationList").empty().html("");
    //    GetValidationsDetail(id);
    //});

    $(document).off("click", "#nextBtn");
    $(document).on("click", "#nextBtn", function (e) {

        if (currentStep == 5) {
            SaveIncidentValidation();
        }

        if (currentStep == 2) {
            var rediusId = $("#step-2").find("#RadiusId").val();
            var severityLevelId = $("#step-2").find("#severityLevelId").val();

            if (severityLevelId == "") {
                SwalErrorAlert("Please select confirmed severity level..!");
                return;
            }

            if (rediusId == "") {
                SwalErrorAlert("Please select discovery perimeter..!");
                return;
            }
        }

        if (currentStep == 3) {
            var selectedTeamCount = document.querySelectorAll(".team-card.selected");
            if (selectedTeamCount.length == 0) {
                SwalErrorAlert("Please select any one response team..!");
                return;
            }
        }

        if (currentStep == 4) {
            var selectPoliciesTaskCount = $(".selectPoliciesTask").find(".task-card").length;
            if (selectPoliciesTaskCount == 0) {
                SwalErrorAlert("Please select any one policy..!");
                return;
            }
        }

        if (currentStep < totalSteps) {
            currentStep++;
            showStep(currentStep);
        }

    });

    $(document).off("click", "#prevBtn");
    $(document).on("click", "#prevBtn", function (e) {
        if (currentStep > 1) {
            currentStep--;
            showStep(currentStep);
        }
    });

    function showStep(step) {

        document.querySelectorAll(".step-content").forEach(el => el.classList.add("d-none"));
        document.querySelector(`#step-${step}`)?.classList.remove("d-none");

        document.querySelectorAll(".steps .step").forEach((el, idx) => {
            el.classList.remove("active", "completed");
            if (idx + 1 < step) el.classList.add("completed");
            if (idx + 1 === step) el.classList.add("active");
        });

        document.getElementById("prevBtn").style.display = (step === 1) ? "none" : "inline-block";
        document.getElementById("nextBtn").innerHTML = (step === totalSteps)
            ? '<i class="fa-solid fa-check"></i> Finish'
            : 'Next <i class="fa-solid fa-arrow-right"></i>';
    }


    $(document).off("click", "#add_task_policy");
    $(document).on("click", "#add_task_policy", function (e) {
        $("#div_Policy").show();
    });

    $(document).off("click", "#cancel_task_policy");
    $(document).on("click", "#cancel_task_policy", function (e) {
        $("#task_policy_title").val('');
        $("#task_policy_description").val('');
        $("#div_Policy").hide();
    });

    $(document).off("click", "#submit_task_policy");
    $(document).on("click", "#submit_task_policy", function (e) {
        e.preventDefault();

        var policy_title = $("#task_policy_title").val();
        var policy_description = $("#task_policy_description").val();

        if (policy_title == "") {
            SwalErrorAlert("Please enter policy title..!");
            return;
        }

        if (policy_description == "") {
            SwalErrorAlert("Please enter policy description title..!");
            return;
        }

        var policyData = {
            Name: $("#task_policy_title").val(),
            Description: $("#task_policy_description").val()
        };

        $.ajax({
            url: '/Validation/SavePolicy', // adjust controller route
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify(policyData),
            async: true,   // ensures async
            beforeSend: function () {
                showLoader($(".main-content"));
            },
            success: function (response) {
                console.log(response);

                if (response && response.Success) {

                    // Get policy details
                    var policyName = response.Request.Name;
                    var policyDesc = response.Request.Description;
                    var policyId = response.Request.Id;
                    var teams = response.AssignTeams;

                    var encodedTeams = JSON.stringify(teams)
                        .replace(/"/g, "&quot;"); // escape double quotes for HTML

                    var ivPolicyList = `<div class="task-card">
                        <h5> ${policyName}</h5>
                                        <p>${policyDesc}</p>
                                        <a href="#" id="add_policy_workflow" data-id="${policyId}"
                                           data-teams="${encodedTeams}"><i class="fa-solid fa-plus"></i> Add to Workflow</a>
                                    </div >`;

                    $(".IVPolicyList").append(ivPolicyList);

                    // ✅ Check if already exists
                    if ($(".selectPoliciesTask .task-card[data-id='" + policyId + "']").length > 0) {
                        return;
                    }

                    // Build team checkboxes dynamically
                    var teamHtml = "";
                    if (teams && teams.length) {
                        $.each(teams, function (i, team) {
                            teamHtml += `<label><input type="checkbox" value="${team.Value}"> ${team.Text}</label>`;
                        });
                    }

                    // Create new task card
                    var newCard = `
                            <div class="task-card" data-id="${policyId}">
                                <div class="task-info">
                                    <h4>${policyName}</h4>
                                    <p>${policyDesc}</p>
                                    <div class="assigned-teams">Assigned Teams:</div>
                                    <div class="checkbox-group">
                                        ${teamHtml}
                                    </div>
                                </div>
                                <div class="task-actions">
                                    <select>
                                        <option value="1">Not Started</option>
                                        <option value="2">In Progress</option>
                                        <option value="3">Completed</option>
                                    </select>
                                    <a href="#" class="remove-link">Remove</a>
                                </div>
                            </div>
                        `;

                    // Append inside Selected Policies & Tasks
                    $(".selectPoliciesTask").append(newCard);

                }

            },
            complete: function () {
                hideLoader($(".main-content"));
            },
            error: function (xhr, status, error) {
                console.error("Error saving policy:", error);
                alert("Failed to save policy.");
                hideLoader($(".main-content"));
            }
        });
    });

    $(document).off("click", "#add_policy_workflow");
    $(document).on("click", "#add_policy_workflow", function (e) {

        e.preventDefault();

        // Get policy details
        var policyName = $(this).closest(".task-card").find("h5").text();
        var policyDesc = $(this).closest(".task-card").find("p").text();
        var policyId = $(this).data("id");
        var teams = $(this).data("teams"); // dynamic teams from server

        // ✅ Check if already exists
        if ($(".selectPoliciesTask .task-card[data-id='" + policyId + "']").length > 0) {
            return;
        }

        // Build team checkboxes dynamically
        var teamHtml = "";
        if (teams && teams.length) {
            $.each(teams, function (i, team) {
                teamHtml += `<label><input type="checkbox" value="${team.Value}"> ${team.Text}</label>`;
            });
        }

        // Create new task card
        var newCard = `
        <div class="task-card" data-id="${policyId}">
            <div class="task-info">
                <h4>${policyName}</h4>
                <p>${policyDesc}</p>
                <div class="assigned-teams">Assigned Teams:</div>
                <div class="checkbox-group">
                    ${teamHtml}
                </div>
            </div>
            <div class="task-actions">
                <select>
                    <option value="1">Not Started</option>
                    <option value="2">In Progress</option>
                    <option value="3">Completed</option>
                </select>
                <a href="#" class="remove-link">Remove</a>
            </div>
        </div>
    `;

        // Append inside Selected Policies & Tasks
        $(".selectPoliciesTask").append(newCard);
    });

    // Remove task
    $(document).on("click", ".remove-link", function (e) {
        e.preventDefault();
        $(this).closest(".task-card").remove();
    });

    $(document).off("click", "#btnSendCommunication");
    $(document).on("click", "#btnSendCommunication", function (e) {
        
        e.preventDefault();
        var recipientsHtml = "";
        var filediv = "";
        var recipientsValue = [];

        var userName = $("#hdn_user_name").val();
        var message = $("#message").val();
        var files = $("#Image")[0].files;
        let timestamp = getFormattedTime();

        var recipientsLength = $(".newCommunication").find('.recipients').find("input[type='checkbox']:checked").length;

        if (recipientsLength > 0) {
            var recipientNames = "";

            var recipients = $(".newCommunication").find('.recipients').find("input[type='checkbox']:checked");
            if (recipients) {
                $.each(recipients, function (i, team) {
                    recipientNames += $(team).attr("data-name").trim() + ", ";
                    recipientsValue.push($(team).val());
                });
            }
            recipientsHtml += `<div class="attachments">
                                        <strong>Sent to:</strong> <i class="fas fa-users"></i>
                                        <a href="#">${recipientNames}</a>
                                    </div>`;
        }

        if (files.length > 0) {
            var filesHtml = "";
            $.each(files, function (i, file) {
                filesHtml += `<i class="fas fa-paperclip"></i>
                                        <a href="#">${file.name}</a>`;
            });

            filediv = `<div class="entry file">
                            <div class="left">
                                <div class="icon-wrapper">
                                    <i class="fas fa-file-alt"></i>
                                </div>
                                <div class="details">
                                    <div class="top">
                                        ${userName} <span class="label">File</span>
                                    </div>
                                    <div>${message}</div>
                                    <div class="attachments">
                                        <strong>Attachments:</strong> ${filesHtml}
                                    </div>
                                    ${recipientsHtml}
                                </div>
                            </div>
                            <div class="timestamp">${timestamp}</div>
                        </div>`;
        }
        else {
            filediv = `<div class="entry note">
                            <div class="left">
                                <div class="icon-wrapper">
                                    <i class="fas fa-comment"></i>
                                </div>
                                <div class="details">
                                    <div class="top">
                                        ${userName} <span class="label">Note</span>
                                    </div>
                                    <div>${message}</div>
                                    ${recipientsHtml}
                                </div>
                            </div>
                            <div class="timestamp">${timestamp}</div>
                        </div>`;
        }

        $(".communicationHistory").append(filediv);

        //sendCommunication.push({
        //    UserName: userName,
        //    Message: message,
        //    TimeStamp: timestamp,
        //    Files: files,
        //    Recipients: recipientsValue.join(","),
        //    MessageType: $("#msgType").val()
        //});


        if (!sendCommunication[index]) {
            sendCommunication[index] = {
                UserName: userName,
                Message: message,
                TimeStamp: timestamp,
                Recipients: recipientsValue.join(","),
                MessageType: $("#msgType").val(),
                Files: [] // 👈 make sure it exists
            };
        }

        // store the real File objects
        sendCommunication[index].Files = files;

        $("#msgType").val(1);
        $('.newCommunication').find(".recipients input[type=checkbox]").prop("checked", false);
        $('#message').val('');
        RemoveImage();
        index++;
    });
});

function getFormattedTime() {
    let now = new Date();

    // Format like: Sep 05, 07:05 PM
    let options = {
        month: "short",   // Sep
        day: "2-digit",   // 05
        hour: "2-digit",  // 07
        minute: "2-digit",// 05
        hour12: true      // AM/PM
    };

    return now.toLocaleString("en-US", options).replace(",", "");
}



//async function GetValidationsList() {
//    try {
//        showLoader($(".main-content"));

//        const response = await fetch("/Validation/GetValidationsList", {
//            method: "GET",
//            headers: {
//                "Content-Type": "application/json",
//                "Accept": "text/html"
//            }
//        });

//        if (!response.ok) throw new Error("Failed to load incident validation list");

//        const content = await response.text();
//        $("#div_ValidationList").empty().html(content);

//    } catch (error) {
//        console.error("Error loading incident validation list:", error);
//    } finally {
//        hideLoader($(".main-content"));
//    }
//}

async function GetValidationsDetail(id) {
    try {
        showLoader($(".main-content"));

        const response = await fetch(`/Validation/GetValidationsDetail?id=${id}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to get incident validation detail by id");

        const content = await response.text();
        $("#div_ValidationDtl").empty().html(content);
        selectAssignTeam();
        $("#div_Policy").hide();

    } catch (error) {
        console.error("Error get incident validation detail by id:", error);
    } finally {
        hideLoader($(".main-content"));
    }
}

function selectAssignTeam() {
    const teamCards = document.querySelectorAll(".team-card");
    //const selectedBox = document.getElementById("selectedBox");
    const selectedCount = document.getElementById("selectedCount");
    const selectedTags = document.getElementById("selectedTags");

    function updateSelectedTeams() {
        const selected = document.querySelectorAll(".team-card.selected");

        if (selectedCount == null) return;

        selectedCount.textContent = `${selected.length} Team${selected.length !== 1 ? "s" : ""} Selected`;

        $("#teamAssigned").text(selected.length);
        $("#estResponseTeam").text(selected.length == 0 ? "N/A" : "15-30 min");



        selectedTags.innerHTML = "";
        selected.forEach(card => {
            const tag = document.createElement("span");
            tag.classList.add("tag");
            tag.textContent = card.dataset.name;
            selectedTags.appendChild(tag);
        });
    }

    teamCards.forEach(card => {
        card.addEventListener("click", () => {
            card.classList.toggle("selected");

            const icon = card.querySelector("i:last-child");
            if (card.classList.contains("selected")) {
                icon.classList.remove("fa-circle", "uncheck-icon");
                icon.classList.add("fa-circle-check", "check-icon");
            } else {
                icon.classList.remove("fa-circle-check", "check-icon");
                icon.classList.add("fa-circle", "uncheck-icon");
            }

            updateSelectedTeams();
        });
    });
}

function ShowImage(input) {
    if (input.files && input.files.length > 0) {
        var preview = $('#image-thumbnails');
        preview.empty(); // clear previous thumbnails if needed

        Array.from(input.files).forEach(function (file) {
            if (file.type.match('image.*')) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $('<img>')
                        .attr('src', e.target.result)
                        .addClass('img-thumbnail me-2 mb-2')
                        .css({ 'max-width': '100px', 'max-height': '100px' });
                    preview.append(img);
                };
                reader.readAsDataURL(file);
            } else {
                var fileIcon = $('<div>')
                    .addClass('file-item me-2 mb-2 p-2 border rounded')
                    .html('<i class="fa fa-file"></i> ' + file.name);
                preview.append(fileIcon);
            }
        });
    }
}
function RemoveImage() {
    document.getElementById("Image").value = "";
    $(".img-thumbnail").remove();
    $("#hidden-image-url").val(null);
}

async function SaveIncidentValidation() {
    try {

        let form = [];
        let formData = new FormData();
        let obj = $("#incident_validation_form")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        let assignTeamsIds = [];
        var policiesData = [];

        var selectedAssignTeams = $("#step-3").find('.responseCard .team-card.selected');
        $.each(selectedAssignTeams, function (i, val) {
            assignTeamsIds.push($(val).attr('data-id'));
        });

        $("#step-4 .selectPoliciesTask .task-card").each(function () {
            var policyCard = $(this);

            // get policy id (you should store it in a hidden input or data-id attr)
            var policyId = policyCard.data("id") || policyCard.find("input[name='PolicyId']").val();

            // get checked teams
            var teams = [];
            policyCard.find(".checkbox-group input[type=checkbox]:checked").each(function () {
                teams.push($(this).val());
            });

            // get dropdown status
            var status = policyCard.find("select").val();

            // push object
            policiesData.push({
                PolicyId: policyId,
                Teams: teams,
                Status: status
            });
        });

        $.each(params, function (i, val) {
            if (val.name === "IVValidation.severityLevelId") {
                formData.append("ConfirmedSeverityLevelId", val.value);
                form.push({ name: val.name, value: val.value });
            }
            else if (val.name === "IVValidation.RadiusId") {
                formData.append("DiscoveryPerimeterId", val.value);
                form.push({ name: val.name, value: val.value });
            }
            else if (val.name === "IVValidation.ValidationNotes") {
                formData.append("ValidationNotes", val.value);
                form.push({ name: val.name, value: val.value });
            }
        });

        formData.append("Id", $("#hdn_Id").val());

        // Add assign Teams
        if (assignTeamsIds.length > 0) {
            formData.append("AssignResponseTeams", assignTeamsIds.join(","));
        }

        if (policiesData.length > 0) {
            formData.append("listPolicyVM", JSON.stringify(policiesData));
        }

        sendCommunication.forEach((c, i) => {
            formData.append(`listSubmitCommunicationVM[${i}].UserName`, c.UserName);
            formData.append(`listSubmitCommunicationVM[${i}].Message`, c.Message);
            formData.append(`listSubmitCommunicationVM[${i}].TimeStamp`, c.TimeStamp);
            formData.append(`listSubmitCommunicationVM[${i}].Recipients`, c.Recipients);
            formData.append(`listSubmitCommunicationVM[${i}].MessageType`, c.MessageType);

            if (c.Files && c.Files.length > 0) {
                Array.from(c.Files).forEach(file => {
                    formData.append(`listSubmitCommunicationVM[${i}].Files`, file);
                });
            }
        });

        showLoader($(".main-content"));

        //console.log(formData);
        console.log(form);

        // Send request
        let response = await fetch("/Validation/SaveIncidentValidation", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            SwalSuccessAlert(result.data);
            //window.location = '/Incidents';
        } else {
            SwalErrorAlert(result.message || "Failed to Save Incident Validation.");
        }
    } catch (error) {
        SwalErrorAlert("Error while Save Incident Validation!");
        console.error(error);
    } finally {
        hideLoader($(".main-content"));
    }
}