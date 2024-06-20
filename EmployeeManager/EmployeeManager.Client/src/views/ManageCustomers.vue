<script setup>
import { ref } from "vue";
import axios from "axios";

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';

const customers = ref([]);
const validation = ref({});
const newCustomer = ref({
    guid: null,
    name: '',
    zip: '',
    city: '',
    street: ''
});
const showForm = ref(false);

const columns = [
    { field: 'guid', header: 'Guid', hidden: true },
    { field: 'name', header: 'Name' },
    { field: 'zip', header: 'PLZ' },
    { field: 'city', header: 'Ort' },
    { field: 'street', header: 'Adresse' },
];

const addNewCustomer = async () => {
    try {
        const response = await axios.post("customers", newCustomer.value);
        newCustomer.value.guid = response.data.guid;
        customers.value.push({ ...newCustomer.value });
        newCustomer.value = {
            guid: null,
            name: '',
            zip: '',
            city: '',
            street: ''
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

const deleteCustomer = async (guid) => {
    if (confirm(`Möchten Sie den Kunden mit der GUID ${guid} wirklich löschen?`)) {
        try {
            await axios.delete(`customers/${guid}`);
            customers.value = customers.value.filter(cust => cust.guid !== guid);
            alert(`Kunde mit der GUID ${guid} wurde erfolgreich gelöscht.`);
        } catch (e) {
            alert(`Fehler beim Löschen des Kunden mit der GUID ${guid}.`);
        }
    }
};

const onCellEditComplete = async (event) => { 
    let { data, newValue, field } = event;
    try {
        data[field] = newValue;
        validation.value = {};
        if (!data.guid) {
            const response = await axios.post("customers", data);
            data.guid = response.data.guid;
        } else {
            await axios.put("customers", data);
        }
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

const fetchCustomers = async () => {
    const response = await axios.get("customers");
    customers.value = response.data;
};

fetchCustomers();
</script>

<template>
    <div class="manageCustomersView">
        <h1>Kunden verwalten</h1>
        <Button icon="pi pi-plus" label="Neuer Kunde" @click="showForm = true"></Button>
        <DataTable :value="customers" editMode="cell" @cell-edit-complete="onCellEditComplete">
            <Column v-for="col of columns" :key="col.field" :field="col.field" :header="col.header" :hidden="col.hidden" sortable>
                <template #body="{ data, field }">
                    {{ data[field] }}
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
                    <Button @click="deleteCustomer(data.guid)">Löschen</Button>
                </template>
            </Column>
        </DataTable>

        <!-- New Customer Form in Modal -->
        <div v-if="showForm" class="modal">
            <div class="modal-content">
                <span class="close" @click="showForm = false">&times;</span>
                <h2>Neuen Kunden hinzufügen</h2>
                <div>
                    <InputText v-model="newCustomer.name" placeholder="Name" />
                    <div class="error">{{ validation.name }}</div>
                </div>
                <div>
                    <InputText v-model="newCustomer.zip" placeholder="PLZ" />
                    <div class="error">{{ validation.zip }}</div>
                </div>
                <div>
                    <InputText v-model="newCustomer.city" placeholder="Ort" />
                    <div class="error">{{ validation.city }}</div>
                </div>
                <div>
                    <InputText v-model="newCustomer.street" placeholder="Adresse" />
                    <div class="error">{{ validation.street }}</div>
                </div>
                <Button icon="pi pi-check" label="Hinzufügen" @click="addNewCustomer"></Button>
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
.newCustomerForm {
    margin-top: 20px;
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
