<script setup>
import { useUserStore } from '@/stores/user'
import { RouterLink, RouterView } from 'vue-router'
import Toolbar from 'primevue/toolbar';
import Button from 'primevue/button';

const user = useUserStore();
</script>

<template>
  <header>
    <nav>
      <Toolbar class="bg-gray-900 shadow-2"
        style="border-radius: 3rem; background-image: linear-gradient(to right, var(--bluegray-500), var(--bluegray-800))">
        <template #start>
          <RouterLink to="/">
            <Button title="Home">
              <img style="height:32px"
                src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAACXBIWXMAAAsTAAALEwEAmpwYAAADSElEQVR4nO2ZSWhUQRCGv0RjVBI1LkQNUUSTg4ILKBHUgCIiqOQgIhEMKJJrjoInDx6CF81RMQgBQUEFBUHw4ALuuJKDBBXBJe6KuEd90tAP2ra7Z0ky82amfqhb1XRVva6qv3pAIBDkCSOBLuAAMIoSwxTgPBBpuQxMo0SwCHhiBB/Lc6CJIscW4Ksj+Fi+A9spQowAOgOB26L6QgVFgknAuQyCj+USUOv5zXTsE4H5wGOPgwNAB9AO/PToPAUWF2oCNgOfPc69BVYZusuBVx7db0BbISWgDNgN/PE4dh+Y5bCrB24FAurSvSTRCRgHnAo4dQYYH7CvAk4G7M8CNUlNwFygz+OMug17gHJDvxI4COzVtyZGudb13aC+JCZgHfAxUMNbLf064Kqh0+MYe5uAL1lMj5wmoAzYCfz2OPEMWGLZrAD6HbqngTGW7gIPa0xEAqqAEwEHrgBTLZvQyFNy0dEjJgMXkpaAOUBv4PAj1tccDRxO0/FeXSJY/aI7KQlYC7z3HPpLl4SJGcDNDJ1X5KnBcXa7JlB5S0CHDtJ14CdggyNZ77Ks4ZfAQocPawIfYNgSMBY4GjjsAdBoNcddgWSlK2qyNDv8adRnhmyVz0OC+hRXWJGTCYZ+NXB8kIHbK/JGh1/VenL47O55GGdGaA5w9HhtVU9b5pcJNcdsRd2kHVms2WrnWJ1t8O2BkaW+yjZLf32ADA2F/HE02BitgYcWV2MOojLFyHlt1WUqMjTU0mVR6hhLgRcpRnPKvlAHXAv8yB1gpqE/UfeAKMfios4K04EbAbvblv//YJmHosZyzMqgoqmP8hB8iDrHpKsnYPcGWOmq9x+B2uu0NrbWwGNHLsVFndMpywG7L3QHyE2Loac6/v4EBG7KXcfOEaNFx+CyO2QqVujmYt4C1QvmGTq1WS4luZCHwGxPElQM1w1dFeM+36tzjf5zQi07Jpr0A2WUYOnXfcmHBh2HSdrSQluKPzOSJB881HlQiApMfNS5ZBIQBahzXhOQKYaTOpdEAqIsz/0PkgDkBiAlgPQApAkiUwAZgwgPQIgQwgQRKozsAsgyhGyDyDqMvAeQ7+evgnkQiSQByA2ICrEEBAKBQCAQUJT4C8w7/l1pz6aIAAAAAElFTkSuQmCC">
              <!-- <font-awesome-icon icon="fa-solid fa-house" /> -->
            </Button>
          </RouterLink>
          <RouterLink to="/customers">
            <Button title="Kunden verwalten">
              <!-- https://icons8.com/icons -->
              <img style="height:32px"
                src="data:image/png;base64,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">
            </Button>
          </RouterLink>
          <RouterLink to="/employees">
            <Button title="Mitarbeiter verwalten">
              <img style="height:32px"
                src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAACXBIWXMAAAsTAAALEwEAmpwYAAAEzUlEQVR4nO2bSYwVRRjHf64ojCtLVMSDGBJF48EFlyjiwYMXHODgyY3Ek4leiEuMC6A3GYVEL8S70YkLeDB4UF6UqCAoGo1LjLhFRXEQBRd4ppL/S8rO6+rqftXv9bz5fklnpt+r+VfVv6u/+vprAMMwDMMwDMMwjMnKbGAN8D5wQIf7/WFg5gC15khrZ0ZrtfppBMuBCaCdc/wM3DAArRXA/oCW62cZA2Y5cEQDehFYDMzQcR3wkr47BFzbR60VkVqHgVEGxBzvCq8KtLtXbb4CTmig1m/ALAbAGu8KF/GK2t7WUK1HGQC71Lm7PYpYorYvNFTLbSx953d1PhLR9iQveDdRy93yfWd/iYGe7O18k0mrVnaqc7erFXG92u6YZFq1slqdvxzRdrPaPtJHLZeqFLGpQKtWZntJ732Bdg+ozb7Ak0RdWi5VyeP+CK3aGVUy2lZKsESxZ0S3R+cKuzZL+6i1LKFW7dykZDTvkWlfiUGm1BpNqFU7s5SM7lAa4XbC7YotM4dEyzCGiIXAGPCRamztguNX3UZPAOflaLoy1UbgU91q7uc5wCnaFL4EvgDGgVuB6Tk6DwGfAH9GjOuA5jCmOdXO8cAG4N+IwbVzjsMqN3WYB7ye0/Yzxaw9wI+Z774H7ugyxlA9MXS4Oa3XHGszb4s6Owg8CVyuGlsRpwFXAs8AX3tF0fk6d5o/qep8BXAu8F7GyAuBS4F79PDf+e7ZzKSd5iLg9IhxzVDbp1RnbGuOtZi4QR241XBRAr1pnhFvymQft0u+KyPPyHx3FHCLVzhwJvbKxcA30nMrMSkLtcQPJjLP8aC3ulxVpAqXeMWD20lj4iHN9QISMqZBup8pONVLckMP/e8A2wq0bpbOd4GNpQzrpbeOhHws0csS6d0lvTcK2oXqfj5vq527rXtlkbR2k5AyNbYYtktvZSID7yyoUpehliJr7ERiOFtvzP7qsnFU7XeB2n1O8+abK/gW0KqgtVJamyr2243pavdHhfF0m0dfDGwBWytobZSWy+c6KYnLvV6L7LfMGGPoNo++GFiVD6V1lZci5ennfV73qmmsgSPKsf4GTtRnd1cwsO5V09gYuFg6bhfOvtwuY2DsGIcuBq6SztM6Pzbz8B/Tb5kxDl0MfD7zzzKuzlREiLxIUzYG7pHO+V7tLmRgHlMyBp4pDfcMfLQ+21rRwNgxDlUMXCoNVzzt7MjuaeSfARs4aWLgY9J4XOc36nxbBf0pGQO3SKPzLnadztdaDIzjBxl1ls4/8OqBvV6geV58HdoY+K00rlFCfUQl+Wk9DthVd17V37s0qZExsJPsusFWZW1mx23rNScVY2BW65fAK9N+reZcxiW6uQcTj9EG4v6rwl6Zd1xFA33jJrTyUpjX62oOFiz3drnq7UT6VQ1M2Xcdq/l/zAWe6/LiumkGtkom+HWt5uiOq8asVk0GDnI1l6JdouNut4n/t2ZgAUWGmYEFmIERpoQwAyNMCWEGRpgSwgyMMCWEGRhhSggzMMKUEGZgIgPblkhXM7DVxTy/BmeJdI9MSQMnEhRZq2gVFTxTatXKeIIia1mtmIJnSq1aWRAosoZKV71qFRU8U2rVztycImtow+hFK7bgmVLLMAzDMAzDMAyDZvEfcamYO/s1Z4wAAAAASUVORK5CYII=">
              <!-- <font-awesome-icon icon="fa-solid fa-users" /> -->
            </Button>
          </RouterLink>
          <RouterLink v-bind:to="`/calendar/${user.userdata.guid}`">
            <Button title="Kalender">
              <img style="height:32px"
                src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAACXBIWXMAAAsTAAALEwEAmpwYAAABj0lEQVR4nO1YwU7CQBDd/8AABzWe9NTqzWTrReLH8RFcSAWBhJvlIhztemjskq4XvdpEmnJes1WJtqR2sGFLnJe8pFmS9+Z1Z6YJhCAQ28eNM20NnOmLYv/27lK3DhiJ6WQmEzqzZ906YKxMP6lbZy2W5+ZebBl2bBlRbJnyO9PG6d+LcvCrjhHF1OwtLk4PCLh4aoTrTCOaNVZn0OJBOtQIl/SsVjjAx5tfL+YdH2WM1Rk0AFiHml1IgEzbfNFp1jPGTqMODgDWoeYbIECecUMOr0cr06E9kpNmY4MAcB1SRgB1zePWVWKoqJ69k81aaAzUKSWAGjRlrt6gonredIg9oE4pAXSSYABrR26AcSGrSIIBON6ArEQLtTv9Hyz7nGEAjjcgK91CDL8DArdQLnCIOxUf4vauf8gYbiGBW+h/z0AbAwjcQrnQvS7Zn2fAFwvdxbI0/aD4n7suD3raC+YZ2oUDPDw+HTIuXitQtFR0eRDee8F+4QBJG83nNdcXXa3t5CfeNrh4BAKBINvCO+WE9Ir/r6VFAAAAAElFTkSuQmCC">
            </Button>
          </RouterLink>
        </template>

        <template #center>
          <div>
            <h3 class="navbarHeader">Custos Mitarbeiterverwaltung</h3>
            <h6 class="navbarHeader">Idriss &bull; Rahmani &bull; Zivkovic</h6>
          </div>
        </template>
        <template #end>
          <h5 class="navbarHeader">Angemeldet als {{ user.userdata.username }}
            <small>({{ user.userdata.guid }})</small>
          </h5>
        </template>
      </Toolbar>
    </nav>
  </header>
  <main>
    <RouterView />
  </main>
</template>

<style scoped>
button {
  margin-right: 1em;
}

.navbarHeader {
  display: block;
  margin: 0;
  padding: 0;
  color: white;
}
</style>
