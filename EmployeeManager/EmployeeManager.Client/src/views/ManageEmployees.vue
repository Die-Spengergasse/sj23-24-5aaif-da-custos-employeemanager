<script setup>
import { ref } from "vue";
import axios from "axios";

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';

const employees = ref([]);
const validation = ref({});
const newEmployee = ref({
    guid: null,
    username: '',
    firstname: '',
    lastname: '',
    birth: ''
});
const showForm = ref(false);

const columns = [
    { field: 'guid', header: 'Guid', hidden: true },
    { field: 'username', header: 'Username' },
    { field: 'firstname', header: 'Vorname' },
    { field: 'lastname', header: 'Nachname' },
    { field: 'birth', header: 'Geburtsdatum' },
];

const addNewEmployee = async () => {
    try {
        const response = await axios.post("employees", newEmployee.value);
        newEmployee.value.guid = response.data.guid;
        employees.value.push({ ...newEmployee.value });
        newEmployee.value = {
            guid: null,
            username: '',
            firstname: '',
            lastname: '',
            birth: ''
        };
        showForm.value = false;
    } catch (e) {
        if (!e.response) alert("Der Server ist nicht erreichbar.");
        else if (!e.response.data.errors) alert(e.response.data);
        else if (e.response.status == 400) {
            validation.value = Object.keys(e.response.data.errors).reduce((prev, key) => {
                const newKey = key.charAt(0).toLowerCase() + key.slice(1);
                prev[newKey] = e.response.data.errors[key][0];
                return prev;
            }, {});
        }
    }
};

const deleteEmployee = async (guid) => {
    if (confirm(`Möchten Sie den Mitarbeiter mit der GUID ${guid} wirklich löschen?`)) {
        try {
            await axios.delete(`employees/${guid}`);
            employees.value = employees.value.filter(emp => emp.guid !== guid);
            alert(`Mitarbeiter mit der GUID ${guid} wurde erfolgreich gelöscht.`);
        } catch (e) {
            alert(`Fehler beim Löschen des Mitarbeiters mit der GUID ${guid}.`);
        }
    }
};

const onCellEditComplete = async (event) => { 
    let { data, newValue, field } = event;
    try {
        data[field] = newValue;
        validation.value = {};
        await axios.put("employees", data);
    } catch (e) {
        if (!e.response) alert("Der Server ist nicht erreichbar.");
        else if (!e.response.data.errors) alert(e.response.data);
        else if (e.response.status == 400) {
            validation.value = Object.keys(e.response.data.errors).reduce((prev, key) => {
                const newKey = key.charAt(0).toLowerCase() + key.slice(1);
                prev[newKey] = e.response.data.errors[key][0];
                return prev;
            }, { guid: data.guid });
        }
    }
};

const fetchEmployees = async () => {
    const response = await axios.get("employees");
    employees.value = response.data.map(d => {
        d.birth = d.birth.substring(0, 10);
        return d;
    });
};

fetchEmployees();
</script>

<template>
    <div class="manageEmployeesView">
        <h1>Mitarbeiter verwalten</h1>
        <Button icon="pi pi-plus" label="Neuen Mitarbeiter hinzufügen" @click="showForm = true"></Button>
        <DataTable :value="employees" editMode="cell" @cell-edit-complete="onCellEditComplete">
            <Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header" :hidden="col.hidden" sortable>
                <template #body="{ data, field }">
                    <template v-if="field == 'birth'">
                        {{ new Date(data[field]).toLocaleDateString() }}
                    </template>
                    <template v-else>
                        {{ data[field] }}
                    </template>
                    <div class="error" v-if="validation.guid == data.guid">
                        {{ validation[field] }}
                    </div>
                </template>
                <template #editor="{ data, field }">
                    <InputText v-model="data[field]" autofocus />
                </template>
            </Column>
            <Column header="Actions">
                <template #body="{ data }">
                    <Button @click="deleteEmployee(data.guid)">Löschen</Button>
                </template>
            </Column>
        </DataTable>

        <!-- New Employee Form in Modal -->
        <div v-if="showForm" class="modal">
            <div class="modal-content">
                <span class="close" @click="showForm = false">&times;</span>
                <h2>Neuen Mitarbeiter hinzufügen</h2>
                <div>
                    <InputText v-model="newEmployee.username" placeholder="Username" />
                    <div class="error">{{ validation.username }}</div>
                </div>
                <div>
                    <InputText v-model="newEmployee.firstname" placeholder="Vorname" />
                    <div class="error">{{ validation.firstname }}</div>
                </div>
                <div>
                    <InputText v-model="newEmployee.lastname" placeholder="Nachname" />
                    <div class="error">{{ validation.lastname }}</div>
                </div>
                <div>
                    <InputText v-model="newEmployee.birth" placeholder="Geburtsdatum (YYYY-MM-DD)" />
                    <div class="error">{{ validation.birth }}</div>
                </div>
                <Button icon="pi pi-check" label="Hinzufügen" @click="addNewEmployee"></Button>
                <Button icon="pi pi-times" label="Abbrechen" @click="showForm = false"></Button>
            </div>
        </div>
    </div>
</template>

<style scoped>
.error {
    color: red;
    font-size: 80%;
}

.modal {
    display: block;
    position: fixed;
    z-index: 1;
    left: 0;
    top: 0;
    width: 100%;
    height: 100%;
    overflow: auto;
    background-color: rgb(0,0,0);
    background-color: rgba(0,0,0,0.4);
}

.modal-content {
    background-color: #fefefe;
    margin: 15% auto;
    padding: 20px;
    border: 1px solid #888;
    width: 80%;
}

.close {
    color: #aaa;
    float: right;
    font-size: 28px;
    font-weight: bold;
}

.close:hover,
.close:focus {
    color: black;
    text-decoration: none;
    cursor: pointer;
}
</style>




