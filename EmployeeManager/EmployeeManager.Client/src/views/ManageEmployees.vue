<script setup>
import axios from "axios";

import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
</script>

<template>
    <div class="manageEmployeesView">
        <h1>Mitarbeiter verwalten</h1>
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
        </DataTable>
    </div>
</template>

<script>
export default {
    data() {
        return {
            employees: null,
            validation: {},
            columns: [
                { field: 'guid', header: 'Guid', hidden: true },
                { field: 'firstname', header: 'Vorname' },
                { field: 'lastname', header: 'Nachname' },
                { field: 'birth', header: 'Gebutsdatum' },
            ]
        };
    },
    async mounted() {
        const response = await axios.get("employees");
        this.employees = response.data.map(d => {
            d.birth = d.birth.substring(0, 10);
            return d;
        });
    },
    methods: {
        async onCellEditComplete(event) {
            let { data, newValue, field } = event;
            //const oldValue = data[field];
            try {
                data[field] = newValue;
                this.validation = {};
                await axios.put("employees", data);
            }
            catch (e) {
                //event.preventDefault();
                if (!e.response) alert("Der Server ist nicht erreichbar.");
                else if (!e.response.data.errors) alert(e.response.data);
                else if (e.response.status == 400) {
                    this.validation = Object.keys(e.response.data.errors).reduce((prev, key) => {
                        const newKey = key.charAt(0).toLowerCase() + key.slice(1);
                        prev[newKey] = e.response.data.errors[key][0];
                        return prev;
                    }, { guid: data.guid });
                }
                //data[field] = oldValue;
            }
        }
    }
};
</script>

<style scoped>
.error {
    color: red;
    font-size: 80%;
}
</style>